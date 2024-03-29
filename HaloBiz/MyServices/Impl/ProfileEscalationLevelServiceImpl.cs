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
using HalobizMigrations.Models.Complaints;

namespace HaloBiz.MyServices.Impl
{
    public class ProfileEscalationLevelServiceImpl : IProfileEscalationLevelService
    {
        private readonly ILogger<ProfileEscalationLevelServiceImpl> _logger;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly IProfileEscalationLevelRepository _profileEscalationLevelRepo;
        private readonly IMapper _mapper;

        public ProfileEscalationLevelServiceImpl(IModificationHistoryRepository historyRepo, IProfileEscalationLevelRepository ProfileEscalationLevelRepo, ILogger<ProfileEscalationLevelServiceImpl> logger, IMapper mapper)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._profileEscalationLevelRepo = ProfileEscalationLevelRepo;
            this._logger = logger;
        }

        public async Task<ApiCommonResponse> AddProfileEscalationLevel(HttpContext context, ProfileEscalationLevelReceivingDTO profileEscalationLevelReceivingDTO)
        {
            //Check If User already is already configured for profile escalation level
           if(await _profileEscalationLevelRepo.AlreadyHasProfileEscalationConfigured(profileEscalationLevelReceivingDTO.UserProfileId))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE,null, "User already has an active profile escalation level configured");
            }

            var profileEscalationLevel = _mapper.Map<ProfileEscalationLevel>(profileEscalationLevelReceivingDTO);
            profileEscalationLevel.CreatedById = context.GetLoggedInUserId();
            var savedprofileEscalationLevel = await _profileEscalationLevelRepo.SaveProfileEscalationLevel(profileEscalationLevel);
            if (savedprofileEscalationLevel == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var profileEscalationLevelTransferDTO = _mapper.Map<ProfileEscalationLevelTransferDTO>(profileEscalationLevel);
            return CommonResponse.Send(ResponseCodes.SUCCESS,profileEscalationLevelTransferDTO);
        }

        public async Task<ApiCommonResponse> DeleteProfileEscalationLevel(long id)
        {
            var profileEscalationLevelToDelete = await _profileEscalationLevelRepo.FindProfileEscalationLevelById(id);
            if (profileEscalationLevelToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _profileEscalationLevelRepo.DeleteProfileEscalationLevel(profileEscalationLevelToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> GetAllHandlerProfileEscalationLevel()
        {
            var profileEscalationLevels = await _profileEscalationLevelRepo.FindAllProfileEscalationLevels();
            var handlersOnly = profileEscalationLevels.Where(x => x.EscalationLevel.Caption.ToLower().Contains("handler")).ToList();
            if (handlersOnly == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var profileEscalationLevelTransferDTO = _mapper.Map<IEnumerable<ProfileEscalationLevelTransferDTO>>(handlersOnly);
            return CommonResponse.Send(ResponseCodes.SUCCESS,profileEscalationLevelTransferDTO);
        }

        public async Task<ApiCommonResponse> GetAllProfileEscalationLevel()
        {
            var profileEscalationLevels = await _profileEscalationLevelRepo.FindAllProfileEscalationLevels();
            if (profileEscalationLevels == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var profileEscalationLevelTransferDTO = _mapper.Map<IEnumerable<ProfileEscalationLevelTransferDTO>>(profileEscalationLevels);
            return CommonResponse.Send(ResponseCodes.SUCCESS,profileEscalationLevelTransferDTO);
        }

        public async Task<ApiCommonResponse> GetProfileEscalationLevelById(long id)
        {
            var profileEscalationLevel = await _profileEscalationLevelRepo.FindProfileEscalationLevelById(id);
            if (profileEscalationLevel == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var profileEscalationLevelTransferDTOs = _mapper.Map<ProfileEscalationLevelTransferDTO>(profileEscalationLevel);
            return CommonResponse.Send(ResponseCodes.SUCCESS,profileEscalationLevelTransferDTOs);
        }

        /*public async Task<ApiCommonResponse> GetProfileEscalationLevelByName(string name)
        {
            var profileEscalationLevel = await _profileEscalationLevelRepo.FindProfileEscalationLevelByName(name);
            if (profileEscalationLevel == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var profileEscalationLevelTransferDTOs = _mapper.Map<ProfileEscalationLevelTransferDTO>(profileEscalationLevel);
            return CommonResponse.Send(ResponseCodes.SUCCESS,profileEscalationLevelTransferDTOs);
        }*/

        public async Task<ApiCommonResponse> UpdateProfileEscalationLevel(HttpContext context, long id, ProfileEscalationLevelReceivingDTO profileEscalationLevelReceivingDTO)
        {
            var profileEscalationLevelToUpdate = await _profileEscalationLevelRepo.FindProfileEscalationLevelById(id);
            if (profileEscalationLevelToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            var summary = $"Initial details before change, \n {profileEscalationLevelToUpdate.ToString()} \n";

            profileEscalationLevelToUpdate.UserProfileId = profileEscalationLevelReceivingDTO.UserProfileId;
            profileEscalationLevelToUpdate.EscalationLevelId = profileEscalationLevelReceivingDTO.EscalationLevelId;
            var updatedprofileEscalationLevel = await _profileEscalationLevelRepo.UpdateProfileEscalationLevel(profileEscalationLevelToUpdate);

            summary += $"Details after change, \n {updatedprofileEscalationLevel.ToString()} \n";

            if (updatedprofileEscalationLevel == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            ModificationHistory history = new ModificationHistory()
            {
                ModelChanged = "profileEscalationLevel",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedprofileEscalationLevel.Id
            };
            await _historyRepo.SaveHistory(history);

            var profileEscalationLevelTransferDTOs = _mapper.Map<ProfileEscalationLevelTransferDTO>(updatedprofileEscalationLevel);
            return CommonResponse.Send(ResponseCodes.SUCCESS,profileEscalationLevelTransferDTOs);
        }
    }
}
