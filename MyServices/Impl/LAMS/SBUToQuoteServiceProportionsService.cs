using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HaloBiz.Data;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs.LAMS;
using HaloBiz.DTOs.TransferDTOs.LAMS;
using HaloBiz.Helpers;
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
        private readonly IMapper _mapper;

        public SBUToQuoteServiceProportionsServiceImpl(
                                        ISBUToQuoteServiceProportionRepository sbuToQuotePropRepo, 
                                        IMapper mapper)
        {
            this._mapper = mapper;
            this._sbuToQuotePropRepo = sbuToQuotePropRepo;
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

            entitiesToSave = SetProportionValue(entitiesToSave, context);

            var savedEntities = await _sbuToQuotePropRepo.SaveSBUToQuoteServiceProportion(entitiesToSave);
            if (savedEntities == null)
            {
                return new ApiResponse(404);
            }
            var sbuToQuoteProportionTransferDTOs = _mapper
                                        .Map<IEnumerable<SBUToQuoteServiceProportionTransferDTO>>(savedEntities);
            return new ApiOkResponse(sbuToQuoteProportionTransferDTOs);
        }
        private IEnumerable<SBUToQuoteServiceProportion> SetProportionValue(IEnumerable<SBUToQuoteServiceProportion> entities, HttpContext context) 
        {
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

        
    }
}