using AutoMapper;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Helpers;
using HalobizMigrations.Models;
using HaloBiz.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HalobizMigrations.Models.Halobiz;

namespace HaloBiz.MyServices.Impl
{
    public class AppReviewServiceImpl : IAppReviewService
    {
        private readonly ILogger<AppReviewServiceImpl> _logger;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly IAppReviewRepository _appReviewRepo;
        private readonly IMapper _mapper;

        public AppReviewServiceImpl(IModificationHistoryRepository historyRepo, IAppReviewRepository AppReviewRepo, ILogger<AppReviewServiceImpl> logger, IMapper mapper)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._appReviewRepo = AppReviewRepo;
            this._logger = logger;
        }

        public async Task<ApiResponse> AddAppReview(HttpContext context, AppReviewReceivingDTO appReviewReceivingDTO)
        {

            var appReview = _mapper.Map<AppReview>(appReviewReceivingDTO);
            appReview.CreatedById = context.GetLoggedInUserId();
            var savedappReview = await _appReviewRepo.SaveAppReview(appReview);
            if (savedappReview == null)
            {
                return new ApiResponse(500);
            }
            var appReviewTransferDTO = _mapper.Map<AppReviewTransferDTO>(appReview);
            return new ApiOkResponse(appReviewTransferDTO);
        }

        public async Task<ApiResponse> DeleteAppReview(long id)
        {
            var appReviewToDelete = await _appReviewRepo.FindAppReviewById(id);
            if (appReviewToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _appReviewRepo.DeleteAppReview(appReviewToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> GetAllAppReview()
        {
            var appReviews = await _appReviewRepo.FindAllAppReviews();
            if (appReviews == null)
            {
                return new ApiResponse(404);
            }
            var appReviewTransferDTO = _mapper.Map<IEnumerable<AppReviewTransferDTO>>(appReviews);
            return new ApiOkResponse(appReviewTransferDTO);
        }

        public async Task<ApiResponse> GetAppReviewById(long id)
        {
            var appReview = await _appReviewRepo.FindAppReviewById(id);
            if (appReview == null)
            {
                return new ApiResponse(404);
            }
            var appReviewTransferDTOs = _mapper.Map<AppReviewTransferDTO>(appReview);
            return new ApiOkResponse(appReviewTransferDTOs);
        }

        public async Task<ApiResponse> UpdateAppReview(HttpContext context, long id, AppReviewReceivingDTO appReviewReceivingDTO)
        {
            var appReviewToUpdate = await _appReviewRepo.FindAppReviewById(id);
            if (appReviewToUpdate == null)
            {
                return new ApiResponse(404);
            }

            var summary = $"Initial details before change, \n {appReviewToUpdate.ToString()} \n";

            appReviewToUpdate.Module = appReviewReceivingDTO.Module;
            appReviewToUpdate.LookAndFeelRating = appReviewReceivingDTO.LookAndFeelRating;
            appReviewToUpdate.FunctionalityRating = appReviewReceivingDTO.FunctionalityRating;

            var updatedappReview = await _appReviewRepo.UpdateAppReview(appReviewToUpdate);

            summary += $"Details after change, \n {updatedappReview.ToString()} \n";

            if (updatedappReview == null)
            {
                return new ApiResponse(500);
            }
            ModificationHistory history = new ModificationHistory()
            {
                ModelChanged = "appReview",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedappReview.Id
            };
            await _historyRepo.SaveHistory(history);

            var appReviewTransferDTOs = _mapper.Map<AppReviewTransferDTO>(updatedappReview);
            return new ApiOkResponse(appReviewTransferDTOs);
        }
    }
}
