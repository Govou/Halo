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
    public class ActivityServiceImpl : IActivityService
    {
        private readonly ILogger<ActivityServiceImpl> _logger;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly IActivityRepository _activityRepo;
        private readonly IMapper _mapper;

        public ActivityServiceImpl(IModificationHistoryRepository historyRepo, IActivityRepository ActivityRepo, ILogger<ActivityServiceImpl> logger, IMapper mapper)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._activityRepo = ActivityRepo;
            this._logger = logger;
        }

        public async Task<ApiCommonResponse> AddActivity(HttpContext context, ActivityReceivingDTO activityReceivingDTO)
        {

            var activity = _mapper.Map<Activity>(activityReceivingDTO);
            activity.CreatedById = context.GetLoggedInUserId();
            var savedactivity = await _activityRepo.SaveActivity(activity);
            if (savedactivity == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var activityTransferDTO = _mapper.Map<ActivityTransferDTO>(activity);
            return CommonResponse.Send(ResponseCodes.SUCCESS,activityTransferDTO);
        }

        public async Task<ApiCommonResponse> DeleteActivity(long id)
        {
            var activityToDelete = await _activityRepo.FindActivityById(id);
            if (activityToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _activityRepo.DeleteActivity(activityToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> GetAllActivity()
        {
            var activitys = await _activityRepo.FindAllActivitys();
            if (activitys == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var activityTransferDTO = _mapper.Map<IEnumerable<ActivityTransferDTO>>(activitys);
            return CommonResponse.Send(ResponseCodes.SUCCESS,activityTransferDTO);
        }

        public async Task<ApiCommonResponse> GetActivityById(long id)
        {
            var activity = await _activityRepo.FindActivityById(id);
            if (activity == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var activityTransferDTOs = _mapper.Map<ActivityTransferDTO>(activity);
            return CommonResponse.Send(ResponseCodes.SUCCESS,activityTransferDTOs);
        }

        public async Task<ApiCommonResponse> GetActivityByName(string name)
        {
            var activity = await _activityRepo.FindActivityByName(name);
            if (activity == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var activityTransferDTOs = _mapper.Map<ActivityTransferDTO>(activity);
            return CommonResponse.Send(ResponseCodes.SUCCESS,activityTransferDTOs);
        }

        public async Task<ApiCommonResponse> UpdateActivity(HttpContext context, long id, ActivityReceivingDTO activityReceivingDTO)
        {
            var activityToUpdate = await _activityRepo.FindActivityById(id);
            if (activityToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            var summary = $"Initial details before change, \n {activityToUpdate.ToString()} \n";

            activityToUpdate.Subject = activityReceivingDTO.Subject;
            activityToUpdate.Description = activityReceivingDTO.Description;
            activityToUpdate.Body = activityReceivingDTO.Body;
            activityToUpdate.ActivityStatus = activityReceivingDTO.ActivityStatus;
            activityToUpdate.ActivityType = activityReceivingDTO.ActivityType;
            var updatedactivity = await _activityRepo.UpdateActivity(activityToUpdate);

            summary += $"Details after change, \n {updatedactivity.ToString()} \n";

            if (updatedactivity == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            ModificationHistory history = new ModificationHistory()
            {
                ModelChanged = "activity",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedactivity.Id
            };
            await _historyRepo.SaveHistory(history);

            var activityTransferDTOs = _mapper.Map<ActivityTransferDTO>(updatedactivity);
            return CommonResponse.Send(ResponseCodes.SUCCESS,activityTransferDTOs);
        }
    }
}
