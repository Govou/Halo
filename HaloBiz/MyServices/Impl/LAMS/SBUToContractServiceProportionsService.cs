using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HalobizMigrations.Data;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs.LAMS;
using HaloBiz.DTOs.TransferDTOs.LAMS;
using HaloBiz.Helpers;
using HalobizMigrations.Models;

using HaloBiz.MyServices.LAMS;
using HaloBiz.Repository;
using HaloBiz.Repository.LAMS;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using HaloBiz.Repository.Impl.LAMS;
using Microsoft.EntityFrameworkCore;
using HaloBiz.DTOs.TransferDTOs;

namespace HaloBiz.MyServices.Impl.LAMS
{
    public interface ISBUToContractServiceProportionsService
    {
        Task<ApiCommonResponse> GetAllSBUProportionForContractService(long quoteServiceId);
        Task<ApiCommonResponse> SaveSBUToQuoteProp(HttpContext context, IEnumerable<SbutoContractServiceProportionReceivingDTO> entities);
    }

    public class SBUToContractServiceProportionsService : ISBUToContractServiceProportionsService
    {
        private readonly ISBUToContractServiceProportionRepository _sbuToServicePropRepo;
        private readonly HalobizContext _context;
        private readonly  ISbuproportionRepository _sbuRepo;
        private readonly IMapper _mapper;
        private readonly IApprovalService _approvalService;


        public SBUToContractServiceProportionsService(
                                        ISbuproportionRepository sbuRepo,
                                        HalobizContext context,
                                        ISBUToContractServiceProportionRepository sbuToQuotePropRepo,
                                        IApprovalService approvalService,
                                        IMapper mapper)
        {
            _mapper = mapper;
            _sbuToServicePropRepo = sbuToQuotePropRepo;
            _context = context;
            _sbuRepo = sbuRepo;
            _approvalService = approvalService;
        }

        public async Task<ApiCommonResponse> GetAllSBUProportionForContractService(long quoteServiceId)
        {
            var sbuQuoteProp = await _sbuToServicePropRepo
                .FindAllSbutoContractServiceProportionByQuoteServiceId(quoteServiceId);
            if (sbuQuoteProp == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            // //SbutoContractServiceProportionTransferDTO
            var sbuQuotePropTransferDTOs = _mapper.Map<IEnumerable<SbutoContractServiceProportionTransferDTO>>(sbuQuoteProp);
            return CommonResponse.Send(ResponseCodes.SUCCESS,sbuQuotePropTransferDTOs);
        }

        public async Task<ApiCommonResponse> SaveSBUToQuoteProp(HttpContext httpcontext, IEnumerable<SbutoContractServiceProportionReceivingDTO> entities)
        {
            var entitiesToSave = _mapper.Map<IEnumerable<SbutoContractServiceProportion>>(entities);
            var defaultService = entities.FirstOrDefault();

            //group according the quote service id
            var filtered = from e in entitiesToSave
                                          group e by e.ContractServiceId into g
                                          select new
                                          {
                                              ContractServiceId = g.Key,
                                              Members = g.Select(x => new SbutoContractServiceProportion
                                              { 
                                                  StrategicBusinessUnitId =  x.StrategicBusinessUnitId,
                                                  UserInvolvedId = x.UserInvolvedId,
                                                  Status = x.Status,
                                                  ContractServiceId = x.ContractServiceId                                                

                                              }).ToList()
                                          };

            List<SbutoContractServiceProportion> entitiesToSaveFiltered = new List<SbutoContractServiceProportion>();

            foreach (var item in filtered)
            {
                var toSave = await SetProportionValue(item.Members, httpcontext);
                entitiesToSaveFiltered.AddRange(toSave);
            }

            var savedEntities = await _sbuToServicePropRepo.SaveSbutoContractServiceProportion(entitiesToSaveFiltered);
            if (savedEntities == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            var sbuToQuoteProportionTransferDTOs = _mapper
                                        .Map<IEnumerable<SbutoContractServiceProportionTransferDTO>>(savedEntities);

            var (isSbuComplete, contract) = await IsSBUComplete(defaultService);
            if (isSbuComplete)
            {
                //add aapprovals and update contract that SBU has been added
                contract.HasAddedSBU = true;
                _context.Contracts.Update(contract);
                 await _context.SaveChangesAsync();

                //set up approvals for the contract services
               var approvedSetup = await _approvalService.SetUpApprovalsForContractCreationEndorsement(contract.Id, httpcontext);
               
                return CommonResponse.Send(ResponseCodes.SUCCESS, sbuToQuoteProportionTransferDTOs, "Users added and sent for approval");

            }

            return CommonResponse.Send(ResponseCodes.SUCCESS,sbuToQuoteProportionTransferDTOs,"User added successfully");
        }

        private async Task<(bool, Contract)> IsSBUComplete(SbutoContractServiceProportionReceivingDTO entity)
        {
            //check how many have been enttered again how many are available
            var contractService = await _context.ContractServices.FindAsync(entity.ContractServiceId);

            var contract =  await _context.Contracts.Where(x => x.Id == contractService.ContractId)
                                .Include(x => x.ContractServices)
                                    .ThenInclude(x => x.SbutoContractServiceProportions)
                                .FirstOrDefaultAsync();
            foreach (var item in contract.ContractServices)
            {
                if(!item.SbutoContractServiceProportions.Any())
                {
                    return (false, null);
                }
            }

            return (true,contract);
        }

        private async Task<IEnumerable<SbutoContractServiceProportion>> SetProportionValue(IEnumerable<SbutoContractServiceProportion> entities, HttpContext context) 
        {
            var quoteServiceId = entities.Select(x => x.ContractServiceId).First();
            var quoteService = await _context.ContractServices.Where(x => x.Id == quoteServiceId)
                                .Include(x=>x.Service)
                                .FirstOrDefaultAsync();
            var sbuProportion = await _sbuRepo.FindSbuproportionByOperatingEntityId(quoteService.Service.OperatingEntityId);
            
            if(sbuProportion != null)
            {
                return SetProportionValueFromOperatingEntity(entities,sbuProportion, context);
            }

            int sumRatio = 0;
            var loggedInUserId = context.GetLoggedInUserId();

            foreach (var entity in entities) 
            {
                if(entity.Status == (int)ProportionStatusType.LeadGeneratorAndClosure)
                {
                    sumRatio += 2;
                }else
                {
                    sumRatio += 1;
                }

            }

            foreach (var entity in entities)
            {
                if(entity.Status == (int)ProportionStatusType.LeadGeneratorAndClosure)
                {
                    entity.Proportion = Math.Round(2.0/sumRatio * 100.00, 2);
                }else
                {
                    entity.Proportion = Math.Round(1.0/sumRatio * 100.00, 2);
                }

                entity.CreatedById = loggedInUserId;
            }

            return entities;
        }

        private IEnumerable<SbutoContractServiceProportion> SetProportionValueFromOperatingEntity(IEnumerable<SbutoContractServiceProportion> entities, Sbuproportion sbuProportion, HttpContext context)
        {
            var loggedInUserId = context.GetLoggedInUserId();
            var closure = sbuProportion.LeadClosureProportion;
            var generation = sbuProportion.LeadGenerationProportion;

            var closureRatio = 0.0;
            var generationRation = 0.0;

            foreach (var entity in entities) 
            {
                if(entity.Status == (int)ProportionStatusType.LeadGeneratorAndClosure)
                {
                    closureRatio += 1;
                    generationRation += 1;
                }else if(entity.Status == (int)ProportionStatusType.LeadClosure)
                {
                    closureRatio += 1;
                }else{
                    generationRation += 1;
                }
            }

            var percentageClosurePerUser = Math.Round(closure / closureRatio, 2); 
            var percentageGenerationPerUser = Math.Round(generation / generationRation, 2); 

            foreach (var entity in entities)
            {
                if(entity.Status == (int)ProportionStatusType.LeadGeneratorAndClosure)
                {
                    entity.Proportion = percentageClosurePerUser + percentageGenerationPerUser;
                }else if(entity.Status == (int)ProportionStatusType.LeadClosure)
                {
                    entity.Proportion = percentageClosurePerUser;
                }else{
                    entity.Proportion = percentageGenerationPerUser;
                }
                entity.CreatedById = loggedInUserId;
            }
            return entities;
        }

        
    }
}