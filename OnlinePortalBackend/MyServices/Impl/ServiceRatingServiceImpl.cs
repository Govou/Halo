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
using Halobiz.Common.DTOs.ApiDTOs;

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

        public async Task<ApiCommonResponse> AddServiceRating(ServiceRatingReceivingDTO serviceRatingReceivingDTO)
        {
            var serviceRating = _mapper.Map<ServiceRating>(serviceRatingReceivingDTO);
            serviceRating.CreatedById = serviceRatingReceivingDTO.CustomerDivisionId;
            var savedServiceRating = await _serviceRatingRepo.SaveServiceRating(serviceRating);
            if(savedServiceRating == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE);
            }
            var serviceRatingTransferDto = _mapper.Map<ServiceRatingTransferDTO>(serviceRating);
            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> AddAppRating(AppRatingReceivingDTO appRating)
        {
            
            var rating = new AppRating
            {
                ApplicationId = appRating.ApplicationId,
                CustomerDivisionId = appRating.CustomerDivisionId,
                CreatedAt = DateTime.Now,
                Rating = appRating.Rating,
                Review = appRating.Review,
                UpdatedAt = DateTime.Now,
               // CreatedById = appRating.CustomerDivisionId

            } ;
            var savedAppRating = await _serviceRatingRepo.SaveAppRating(rating);
            if (savedAppRating == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE);
            }
          //  var serviceRatingTransferDto = _mapper.Map<ServiceRatingTransferDTO>(serviceRating);
            return CommonResponse.Send(ResponseCodes.SUCCESS);
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
            return new ApiOkResponse(serviceRating);
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

        public async Task<ApiResponse> FindAllAppRatings()
        {
            var appRatingsList = new List<AppRatingTransferDTO>();
            var appRatings = await _serviceRatingRepo.FindAllAppRatings();
            if (appRatings == null)
            {
                return new ApiResponse(404);
            }

            foreach (var app in appRatings)
            {
                appRatingsList.Add(new AppRatingTransferDTO
                {
                    ApplicationId = (long)app.ApplicationId,
                    ApplicationName = app.Application.Caption,
                    CustomerName = app.CustomerDivision.DivisionName,
                    Rating = app.Rating,
                    Review = app.Review,
                    DateRated = app.CreatedAt,
                    CustomerDivisionId = app.CustomerDivisionId

                });
            }
           // var serviceRatingsTransferDto = _mapper.Map<IEnumerable<AppRatingReceivingDTO>>(serviceRatings);
            return new ApiOkResponse(appRatingsList);
        }

        public async Task<ApiResponse> FindAllApplications()
        {
            var applications = await _serviceRatingRepo.FindAllApplications();
            if (applications == null)
            {
                return new ApiResponse(404);
            }
            return new ApiOkResponse(applications);

        }

    }
}