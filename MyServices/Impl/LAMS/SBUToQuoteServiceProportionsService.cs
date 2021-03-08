using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HaloBiz.Data;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs.LAMS;
using HaloBiz.DTOs.TransferDTOs.LAMS;
using HaloBiz.Helpers;
using HaloBiz.Model;
using HaloBiz.Model.LAMS;
using HaloBiz.MyServices.LAMS;
using HaloBiz.Repository;
using HaloBiz.Repository.LAMS;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace HaloBiz.MyServices.Impl.LAMS
{
    public class SBUToQuoteServiceProportionsServiceImpl : ISBUToQuoteServiceProportionsService
    {
        private readonly ISBUToQuoteServiceProportionRepository _sbuToQuotePropRepo;
        private readonly IQuoteServiceRepository _quoteServiceRepository;
        private readonly  ISBUProportionRepository _sbuRepo;
        private readonly IMapper _mapper;

        public SBUToQuoteServiceProportionsServiceImpl(
                                        ISBUProportionRepository sbuRepo,
                                        IQuoteServiceRepository quoteServiceRepository,
                                        ISBUToQuoteServiceProportionRepository sbuToQuotePropRepo, 
                                        IMapper mapper)
        {
            this._mapper = mapper;
            this._sbuToQuotePropRepo = sbuToQuotePropRepo;
            this._quoteServiceRepository = quoteServiceRepository;
            this._sbuRepo = sbuRepo;
        }

        public async Task<ApiResponse> GetAllSBUQuoteProportionForQuoteService(long quoteServiceId)
        {
            var sbuQuoteProp = await _sbuToQuotePropRepo
                .FindAllSBUToQuoteServiceProportionByQuoteServiceId(quoteServiceId);
            if (sbuQuoteProp == null)
            {
                return new ApiResponse(404);
            }
            var sbuQuotePropTransferDTOs = _mapper.Map<IEnumerable<SBUToQuoteServiceProportionTransferDTO>>(sbuQuoteProp);
            return new ApiOkResponse(sbuQuotePropTransferDTOs);
        }

        public async Task<ApiResponse> SaveSBUToQuoteProp(HttpContext context, IEnumerable<SBUToQuoteServiceProportionReceivingDTO> entities)
        {

            var entitiesToSave = _mapper.Map<IEnumerable<SBUToQuoteServiceProportion>>(entities);

            entitiesToSave = await SetProportionValue(entitiesToSave, context);

            var savedEntities = await _sbuToQuotePropRepo.SaveSBUToQuoteServiceProportion(entitiesToSave);
            if (savedEntities == null)
            {
                return new ApiResponse(404);
            }
            var sbuToQuoteProportionTransferDTOs = _mapper
                                        .Map<IEnumerable<SBUToQuoteServiceProportionTransferDTO>>(savedEntities);
            return new ApiOkResponse(sbuToQuoteProportionTransferDTOs);
        }
        private async Task<IEnumerable<SBUToQuoteServiceProportion>> SetProportionValue(IEnumerable<SBUToQuoteServiceProportion> entities, HttpContext context) 
        {
            var quoteServiceId = entities.Select(x => x.QuoteServiceId).First();
            var quoteService = await  _quoteServiceRepository.FindQuoteServiceById(quoteServiceId);
            var sbuProportion = await _sbuRepo.FindSBUProportionByOperatingEntityId(quoteService.Service.OperatingEntityId);
            
            if(sbuProportion != null)
            {
                return SetProportionValueFromOperatingEntity(entities,sbuProportion, context);
            }

            int sumRatio = 0;
            var loggedInUserId = context.GetLoggedInUserId();

            foreach (var entity in entities) 
            {
                if(entity.Status == ProportionStatusType.LeadGeneratorAndClosure)
                {
                    sumRatio += 2;
                }else
                {
                    sumRatio += 1;
                }

            }

            foreach (var entity in entities)
            {
                if(entity.Status == ProportionStatusType.LeadGeneratorAndClosure)
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

        private IEnumerable<SBUToQuoteServiceProportion> SetProportionValueFromOperatingEntity(IEnumerable<SBUToQuoteServiceProportion> entities, SBUProportion sbuProportion, HttpContext context)
        {
            var loggedInUserId = context.GetLoggedInUserId();
            var closure = sbuProportion.LeadClosureProportion;
            var generation = sbuProportion.LeadGenerationProportion;

            var closureRatio = 0.0;
            var generationRation = 0.0;

            foreach (var entity in entities) 
            {
                if(entity.Status == ProportionStatusType.LeadGeneratorAndClosure)
                {
                    closureRatio += 1;
                    generationRation += 1;
                }else if(entity.Status == ProportionStatusType.LeadClosure)
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
                if(entity.Status == ProportionStatusType.LeadGeneratorAndClosure)
                {
                    entity.Proportion = percentageClosurePerUser + percentageGenerationPerUser;
                }else if(entity.Status == ProportionStatusType.LeadClosure)
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