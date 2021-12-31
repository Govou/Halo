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
    public class AppFeedbackServiceImpl : IAppFeedbackService
    {
        private readonly ILogger<AppFeedbackServiceImpl> _logger;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly IAppFeedbackRepository _appFeedbackRepo;
        private readonly IMapper _mapper;

        public AppFeedbackServiceImpl(IModificationHistoryRepository historyRepo, IAppFeedbackRepository AppFeedbackRepo, ILogger<AppFeedbackServiceImpl> logger, IMapper mapper)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._appFeedbackRepo = AppFeedbackRepo;
            this._logger = logger;
        }

        public async Task<ApiCommonResponse> AddAppFeedback(HttpContext context, AppFeedbackReceivingDTO appFeedbackReceivingDTO)
        {

            var appFeedback = _mapper.Map<AppFeedback>(appFeedbackReceivingDTO);
            appFeedback.CreatedById = context.GetLoggedInUserId();
            var savedappFeedback = await _appFeedbackRepo.SaveAppFeedback(appFeedback);
            if (savedappFeedback == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var appFeedbackTransferDTO = _mapper.Map<AppFeedbackTransferDTO>(appFeedback);
            return new ApiOkResponse(appFeedbackTransferDTO);
        }

        public async Task<ApiCommonResponse> DeleteAppFeedback(long id)
        {
            var appFeedbackToDelete = await _appFeedbackRepo.FindAppFeedbackById(id);
            if (appFeedbackToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _appFeedbackRepo.DeleteAppFeedback(appFeedbackToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> GetAllAppFeedback()
        {
            var appFeedbacks = await _appFeedbackRepo.FindAllAppFeedbacks();
            if (appFeedbacks == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var appFeedbackTransferDTO = _mapper.Map<IEnumerable<AppFeedbackTransferDTO>>(appFeedbacks);
            return new ApiOkResponse(appFeedbackTransferDTO);
        }

        public async Task<ApiCommonResponse> GetAppFeedbackById(long id)
        {
            var appFeedback = await _appFeedbackRepo.FindAppFeedbackById(id);
            if (appFeedback == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var appFeedbackTransferDTOs = _mapper.Map<AppFeedbackTransferDTO>(appFeedback);
            return new ApiOkResponse(appFeedbackTransferDTOs);
        }

        public async Task<ApiCommonResponse> UpdateAppFeedback(HttpContext context, long id, AppFeedbackReceivingDTO appFeedbackReceivingDTO)
        {
            var appFeedbackToUpdate = await _appFeedbackRepo.FindAppFeedbackById(id);
            if (appFeedbackToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            var summary = $"Initial details before change, \n {appFeedbackToUpdate.ToString()} \n";

            appFeedbackToUpdate.FeedbackType = appFeedbackReceivingDTO.FeedbackType;
            appFeedbackToUpdate.Description = appFeedbackReceivingDTO.Description;
            appFeedbackToUpdate.DocumentsUrl = appFeedbackReceivingDTO.DocumentsUrl;

            var updatedappFeedback = await _appFeedbackRepo.UpdateAppFeedback(appFeedbackToUpdate);

            summary += $"Details after change, \n {updatedappFeedback.ToString()} \n";

            if (updatedappFeedback == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            ModificationHistory history = new ModificationHistory()
            {
                ModelChanged = "appFeedback",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedappFeedback.Id
            };
            await _historyRepo.SaveHistory(history);

            var appFeedbackTransferDTOs = _mapper.Map<AppFeedbackTransferDTO>(updatedappFeedback);
            return new ApiOkResponse(appFeedbackTransferDTOs);
        }
    }
}
