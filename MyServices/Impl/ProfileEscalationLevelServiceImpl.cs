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

        public async Task<ApiResponse> AddProfileEscalationLevel(HttpContext context, ProfileEscalationLevelReceivingDTO profileEscalationLevelReceivingDTO)
        {
            //Check If User already is already configured for profile escalation level
           if(await _profileEscalationLevelRepo.AlreadyHasProfileEscalationConfigured(profileEscalationLevelReceivingDTO.UserProfileId))
            {
                return new ApiResponse(500, "User already has an active profile escalation level configured");
            }

            var profileEscalationLevel = _mapper.Map<ProfileEscalationLevel>(profileEscalationLevelReceivingDTO);
            profileEscalationLevel.CreatedById = context.GetLoggedInUserId();
            var savedprofileEscalationLevel = await _profileEscalationLevelRepo.SaveProfileEscalationLevel(profileEscalationLevel);
            if (savedprofileEscalationLevel == null)
            {
                return new ApiResponse(500);
            }
            var profileEscalationLevelTransferDTO = _mapper.Map<ProfileEscalationLevelTransferDTO>(profileEscalationLevel);
            return new ApiOkResponse(profileEscalationLevelTransferDTO);
        }

        public async Task<ApiResponse> DeleteProfileEscalationLevel(long id)
        {
            var profileEscalationLevelToDelete = await _profileEscalationLevelRepo.FindProfileEscalationLevelById(id);
            if (profileEscalationLevelToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _profileEscalationLevelRepo.DeleteProfileEscalationLevel(profileEscalationLevelToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> GetAllProfileEscalationLevel()
        {
            var profileEscalationLevels = await _profileEscalationLevelRepo.FindAllProfileEscalationLevels();
            if (profileEscalationLevels == null)
            {
                return new ApiResponse(404);
            }
            var profileEscalationLevelTransferDTO = _mapper.Map<IEnumerable<ProfileEscalationLevelTransferDTO>>(profileEscalationLevels);
            return new ApiOkResponse(profileEscalationLevelTransferDTO);
        }

        public async Task<ApiResponse> GetProfileEscalationLevelById(long id)
        {
            var profileEscalationLevel = await _profileEscalationLevelRepo.FindProfileEscalationLevelById(id);
            if (profileEscalationLevel == null)
            {
                return new ApiResponse(404);
            }
            var profileEscalationLevelTransferDTOs = _mapper.Map<ProfileEscalationLevelTransferDTO>(profileEscalationLevel);
            return new ApiOkResponse(profileEscalationLevelTransferDTOs);
        }

        /*public async Task<ApiResponse> GetProfileEscalationLevelByName(string name)
        {
            var profileEscalationLevel = await _profileEscalationLevelRepo.FindProfileEscalationLevelByName(name);
            if (profileEscalationLevel == null)
            {
                return new ApiResponse(404);
            }
            var profileEscalationLevelTransferDTOs = _mapper.Map<ProfileEscalationLevelTransferDTO>(profileEscalationLevel);
            return new ApiOkResponse(profileEscalationLevelTransferDTOs);
        }*/

        public async Task<ApiResponse> UpdateProfileEscalationLevel(HttpContext context, long id, ProfileEscalationLevelReceivingDTO profileEscalationLevelReceivingDTO)
        {
            var profileEscalationLevelToUpdate = await _profileEscalationLevelRepo.FindProfileEscalationLevelById(id);
            if (profileEscalationLevelToUpdate == null)
            {
                return new ApiResponse(404);
            }

            var summary = $"Initial details before change, \n {profileEscalationLevelToUpdate.ToString()} \n";

            profileEscalationLevelToUpdate.UserProfileId = profileEscalationLevelReceivingDTO.UserProfileId;
            profileEscalationLevelToUpdate.EscalationLevelId = profileEscalationLevelReceivingDTO.EscalationLevelId;
            var updatedprofileEscalationLevel = await _profileEscalationLevelRepo.UpdateProfileEscalationLevel(profileEscalationLevelToUpdate);

            summary += $"Details after change, \n {updatedprofileEscalationLevel.ToString()} \n";

            if (updatedprofileEscalationLevel == null)
            {
                return new ApiResponse(500);
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
            return new ApiOkResponse(profileEscalationLevelTransferDTOs);
        }
    }
}
