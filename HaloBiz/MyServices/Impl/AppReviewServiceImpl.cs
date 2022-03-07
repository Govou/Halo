using AutoMapper;
using Halobiz.Common.DTOs.ApiDTOs;
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

        public async Task<ApiCommonResponse> AddAppReview(HttpContext context, AppReviewReceivingDTO appReviewReceivingDTO)
        {

            var appReview = _mapper.Map<AppReview>(appReviewReceivingDTO);
            appReview.CreatedById = context.GetLoggedInUserId();
            var savedappReview = await _appReviewRepo.SaveAppReview(appReview);
            if (savedappReview == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var appReviewTransferDTO = _mapper.Map<AppReviewTransferDTO>(appReview);
            return CommonResponse.Send(ResponseCodes.SUCCESS,appReviewTransferDTO);
        }

        public async Task<ApiCommonResponse> DeleteAppReview(long id)
        {
            var appReviewToDelete = await _appReviewRepo.FindAppReviewById(id);
            if (appReviewToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _appReviewRepo.DeleteAppReview(appReviewToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> GetAllAppReview()
        {
            var appReviews = await _appReviewRepo.FindAllAppReviews();
            if (appReviews == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var appReviewTransferDTO = _mapper.Map<IEnumerable<AppReviewTransferDTO>>(appReviews);
            return CommonResponse.Send(ResponseCodes.SUCCESS,appReviewTransferDTO);
        }

        public async Task<ApiCommonResponse> GetAppReviewById(long id)
        {
            var appReview = await _appReviewRepo.FindAppReviewById(id);
            if (appReview == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var appReviewTransferDTOs = _mapper.Map<AppReviewTransferDTO>(appReview);
            return CommonResponse.Send(ResponseCodes.SUCCESS,appReviewTransferDTOs);
        }

        public async Task<ApiCommonResponse> UpdateAppReview(HttpContext context, long id, AppReviewReceivingDTO appReviewReceivingDTO)
        {
            var appReviewToUpdate = await _appReviewRepo.FindAppReviewById(id);
            if (appReviewToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            var summary = $"Initial details before change, \n {appReviewToUpdate.ToString()} \n";

            appReviewToUpdate.Module = appReviewReceivingDTO.Module;
            appReviewToUpdate.LookAndFeelRating = appReviewReceivingDTO.LookAndFeelRating;
            appReviewToUpdate.FunctionalityRating = appReviewReceivingDTO.FunctionalityRating;

            var updatedappReview = await _appReviewRepo.UpdateAppReview(appReviewToUpdate);

            summary += $"Details after change, \n {updatedappReview.ToString()} \n";

            if (updatedappReview == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
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
            return CommonResponse.Send(ResponseCodes.SUCCESS,appReviewTransferDTOs);
        }
    }
}
