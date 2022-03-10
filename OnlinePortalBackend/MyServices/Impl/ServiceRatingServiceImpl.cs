using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using OnlinePortalBackend.DTOs.ApiDTOs;
using OnlinePortalBackend.DTOs.ReceivingDTOs;
using OnlinePortalBackend.DTOs.TransferDTOs;
using OnlinePortalBackend.Helpers;
using HalobizMigrations.Models.OnlinePortal;
using OnlinePortalBackend.Repository;

namespace OnlinePortalBackend.MyServices.Impl
{
    public class ServiceRatingServiceImpl : IServiceRatingService
    {
        private readonly IServiceRatingRepository _serviceRatingRepo;
        private readonly IMapper _mapper;
        private readonly IModificationHistoryRepository _historyRepo;
        public ServiceRatingServiceImpl(IServiceRatingRepository serviceRatingRepo, 
            IMapper mapper, 
            IModificationHistoryRepository historyRepo)
        {
            _mapper = mapper;
            _serviceRatingRepo = serviceRatingRepo;
            _historyRepo = historyRepo;
        }

        public async Task<ApiResponse> AddServiceRating(HttpContext context, ServiceRatingReceivingDTO serviceRatingReceivingDTO)
        {
            var serviceRating = _mapper.Map<ServiceRating>(serviceRatingReceivingDTO);
            serviceRating.CreatedById = context.GetLoggedInUserId();
            var savedServiceRating = await _serviceRatingRepo.SaveServiceRating(serviceRating);
            if(savedServiceRating == null)
            {
                return new ApiResponse(500);
            }
            var serviceRatingTransferDto = _mapper.Map<ServiceRatingTransferDTO>(serviceRating);
            return new ApiOkResponse(serviceRatingTransferDto);
        }

        public async Task<ApiResponse> GetReviewHistoryByServiceId(long id)
        {
            var serviceRating = await _serviceRatingRepo.GetReviewHistoryByServiceId(id);
            if (serviceRating == null)
            {
                return new ApiResponse(404);
            }
            var serviceRatingTransferDto = _mapper.Map<IEnumerable<ServiceRatingTransferDTO>>(serviceRating);
            return new ApiOkResponse(serviceRatingTransferDto);
        }

        public async Task<ApiResponse> GetMyServiceRatings(HttpContext context)
        {
            var serviceRating = await _serviceRatingRepo.FindServiceRatingsByUserId(context.GetLoggedInUserId());
            if (serviceRating == null)
            {
                return new ApiResponse(404);
            }
            var serviceRatingTransferDto = _mapper.Map<IEnumerable<ServiceRatingTransferDTO>>(serviceRating);
            return new ApiOkResponse(serviceRatingTransferDto);
        }

        public async Task<ApiResponse> FindServiceRatingById(long id)
        {
            var serviceRating = await _serviceRatingRepo.FindServiceRatingById(id);
            if(serviceRating == null)
            {
                return new ApiResponse(404);
            }
            var serviceRatingTransferDto = _mapper.Map<ServiceRatingTransferDTO>(serviceRating);
            return new ApiOkResponse(serviceRatingTransferDto);
        }        

        public async Task<ApiResponse> FindAllServiceRatings()
        {
            var serviceRatings = await _serviceRatingRepo.FindAllServiceRatings();
            if(serviceRatings == null )
            {
                return new ApiResponse(404);
            }
            var serviceRatingsTransferDto = _mapper.Map<IEnumerable<ServiceRatingTransferDTO>>(serviceRatings);
            return new ApiOkResponse(serviceRatingsTransferDto);
        }

        public async Task<ApiResponse> UpdateServiceRating(HttpContext context, long serviceRatingId, ServiceRatingReceivingDTO serviceRatingReceivingDTO)
        {
            var serviceRatingToUpdate = await _serviceRatingRepo.FindServiceRatingById(serviceRatingId);
            if(serviceRatingToUpdate == null)
            {
                return new ApiResponse(404);
            }
            var summary = $"Initial details before change, \n {serviceRatingToUpdate.ToString()} \n" ;
            serviceRatingToUpdate.Rating = serviceRatingReceivingDTO.Rating;

            summary += $"Details after change, \n {serviceRatingToUpdate} \n";

            var updatedServiceRating = await _serviceRatingRepo.UpdateServiceRating(serviceRatingToUpdate);

            if(updatedServiceRating == null)
            {
                return new ApiResponse(500);
            }      

            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "ServiceRating",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedServiceRating.Id
            };

            await _historyRepo.SaveHistory(history);

            var serviceRatingTransferDto = _mapper.Map<ServiceRatingTransferDTO>(updatedServiceRating);
            return new ApiOkResponse(serviceRatingTransferDto);
        }

        public async Task<ApiResponse> DeleteServiceRating(long serviceRatingId)
        {
            var serviceRatingToDelete = await _serviceRatingRepo.FindServiceRatingById(serviceRatingId);
            if(serviceRatingToDelete == null)
            {
                return new ApiResponse(404);
            }

            if(!await _serviceRatingRepo.RemoveServiceRating(serviceRatingToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }
    }
}