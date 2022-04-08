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

        public SBUToContractServiceProportionsService(
                                        ISbuproportionRepository sbuRepo,
                                        HalobizContext context,
                                        ISBUToContractServiceProportionRepository sbuToQuotePropRepo, 
                                        IMapper mapper)
        {
            _mapper = mapper;
            _sbuToServicePropRepo = sbuToQuotePropRepo;
            _context = context;
            _sbuRepo = sbuRepo;
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

        public async Task<ApiCommonResponse> SaveSBUToQuoteProp(HttpContext context, IEnumerable<SbutoContractServiceProportionReceivingDTO> entities)
        {

            var entitiesToSave = _mapper.Map<IEnumerable<SbutoContractServiceProportion>>(entities);

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
                var toSave = await SetProportionValue(item.Members, context);
                entitiesToSaveFiltered.AddRange(toSave);
            }

            var savedEntities = await _sbuToServicePropRepo.SaveSbutoContractServiceProportion(entitiesToSaveFiltered);
            if (savedEntities == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            var sbuToQuoteProportionTransferDTOs = _mapper
                                        .Map<IEnumerable<SbutoContractServiceProportionTransferDTO>>(savedEntities);
           
            return CommonResponse.Send(ResponseCodes.SUCCESS,sbuToQuoteProportionTransferDTOs);
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