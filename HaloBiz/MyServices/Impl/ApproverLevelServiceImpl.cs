using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Helpers;
using HalobizMigrations.Models;
using HaloBiz.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace HaloBiz.MyServices.Impl
{
    public class ApproverLevelServiceImpl : IApproverLevelService
    {
        private readonly ILogger<ApproverLevelServiceImpl> _logger;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly IApproverLevelRepository _approverLevelRepo;
        private readonly IMapper _mapper;

        public ApproverLevelServiceImpl(IModificationHistoryRepository historyRepo, IApproverLevelRepository approverLevelRepo, ILogger<ApproverLevelServiceImpl> logger, IMapper mapper)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._approverLevelRepo = approverLevelRepo;
            this._logger = logger;
        }
        public async  Task<ApiCommonResponse> AddApproverLevel(HttpContext context, ApproverLevelReceivingDTO approverLevelReceivingDTO)
        {
            var approverLevel = _mapper.Map<ApproverLevel>(approverLevelReceivingDTO);
            approverLevel.CreatedById = context.GetLoggedInUserId();
            var savedApproverLevel = await _approverLevelRepo.SaveApproverLevel(approverLevel);
            if (savedApproverLevel == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var approverLevelTransferDTO = _mapper.Map<ApproverLevelTransferDTO>(approverLevel);
            return CommonResponse.Send(ResponseCodes.SUCCESS,approverLevelTransferDTO);
        }

        public async Task<ApiCommonResponse> CreateApprovingLevelOffice(HttpContext context, ApprovingLevelOfficeReceivingDTO model)
        {
            long userProfileID = context.GetLoggedInUserId();
            if (model.OfficersIds.Count <= 0) return CommonResponse.Send(ResponseCodes.SYSTEM_ERROR_OCCURRED, "Err: you must assign at least one approving officer when creating an approving office");
            var lastOffice = await _approverLevelRepo.GetLastApprovingLevelOffice();
            ApprovingLevelOffice approvingLevelOffice = new()
            {
                UserId = model.UserId,
                CreatedById = userProfileID,
                CreatedAt = System.DateTime.Now,
                ApprovingOfficers = new List<ApprovingLevelOfficer>()
            };
            foreach (var officerId in model.OfficersIds)
                approvingLevelOffice.ApprovingOfficers.Add(new ApprovingLevelOfficer()
                {
                    UserId = officerId,
                    CreatedById = userProfileID,
                    CreatedAt = System.DateTime.Now
                });

            var savedApprovingLevelOffice = await _approverLevelRepo.SaveApprovingLevelOffice(approvingLevelOffice);
            if (!savedApprovingLevelOffice)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            return CommonResponse.Send(ResponseCodes.SUCCESS, true);
        }

        public async Task<ApiCommonResponse> DeleteApproverLevel(long id)
        {
            var approverLevelToDelete = await _approverLevelRepo.FindApproverLevelById(id);
            if(approverLevelToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            if (!await _approverLevelRepo.DeleteApproverLevel(approverLevelToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> GetAllApproverLevel()
        {
            var approverLevel = await _approverLevelRepo.GetApproverLevels();
            if (approverLevel == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var approverLevelTransferDTO = _mapper.Map<IEnumerable<ApproverLevelTransferDTO>>(approverLevel);
            return CommonResponse.Send(ResponseCodes.SUCCESS,approverLevelTransferDTO);
        }

        public  async Task<ApiCommonResponse> UpdateApproverLevel(HttpContext context, long id, ApproverLevelReceivingDTO approverLevelReceivingDTO)
        {
            var approverLevelToUpdate = await _approverLevelRepo.FindApproverLevelById(id);
            if (approverLevelToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            
            var summary = $"Initial details before change, \n {approverLevelToUpdate.ToString()} \n" ;

            approverLevelToUpdate.Caption = approverLevelReceivingDTO.Caption;
            approverLevelToUpdate.Alias = approverLevelReceivingDTO.Alias;
            approverLevelToUpdate.Description = approverLevelReceivingDTO.Description;
            var updatedApproverLevel = await _approverLevelRepo.UpdateApproverLevel(approverLevelToUpdate);

            summary += $"Details after change, \n {updatedApproverLevel.ToString()} \n";

            if (updatedApproverLevel == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "ApproverLevel",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedApproverLevel.Id
            };

            await _historyRepo.SaveHistory(history);

            var approverLevelTransferDTOs = _mapper.Map<ApproverLevelTransferDTO>(updatedApproverLevel);
            return CommonResponse.Send(ResponseCodes.SUCCESS,approverLevelTransferDTOs);
        }

        public async Task<ApiCommonResponse> GetAllApprovingLevelOffices()
        {
            var approvingLevelOffices = await _approverLevelRepo.GetApprovingLevelOffices();
            var approvingLevelTransferDTO = _mapper.Map<IEnumerable<ApprovingLevelOfficeTransferDTO>>(approvingLevelOffices);
            return CommonResponse.Send(ResponseCodes.SUCCESS, approvingLevelTransferDTO);
        }

        public async Task<ApiCommonResponse> DeleteApprovingLevelOffice(long id)
        {
            var isDeleted = await _approverLevelRepo.DeleteApprovingLevelOffice(id);
            if (!isDeleted)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }
            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> UpdateApprovingLevelOffice(HttpContext context, long id, ApprovingLevelOfficeReceivingDTO model)
        {
            long userProfileID = context.GetLoggedInUserId();
            if (model.OfficersIds.Count <= 0) return CommonResponse.Send(ResponseCodes.FAILURE, "Err: you must assign at least one approving officer when updating an approving office");
            ApprovingLevelOffice approvingOffice = await _approverLevelRepo.FindApprovingLevelOfficeByID(id);
            if (approvingOffice == null) return CommonResponse.Send(ResponseCodes.FAILURE, "Err: Approving Office does not exists");
            List<ApprovingLevelOfficer> approvingOfficers = await _approverLevelRepo.FindApprovingLevelOfficersByOfficeID(approvingOffice.Id);
            List<ApprovingLevelOfficer> officersToRemove = new();
            foreach (var appOfficer in approvingOfficers)
            {
                if (!model.OfficersIds.Any(x => x == appOfficer.UserId))
                {
                    officersToRemove.Add(appOfficer);
                }
                else
                {
                    var officerIdToRemove = model.OfficersIds.Single(x => x == appOfficer.UserId);
                    model.OfficersIds.Remove(officerIdToRemove);
                }
            }
            if (officersToRemove.Count > 0)
                await _approverLevelRepo.RemoveApprovingLevelOfficers(officersToRemove);

            List<ApprovingLevelOfficer> newOfficers = new();
            foreach (var officerId in model.OfficersIds)
            {
                ApprovingLevelOfficer approvingOfficer = new()
                {
                    UserId = officerId,
                    CreatedById = userProfileID,
                    CreatedAt = System.DateTime.Now,
                    ApprovingLevelOfficeId = approvingOffice.Id
                };
                newOfficers.Add(approvingOfficer);
            }
            await _approverLevelRepo.SaveApprovingLevelOfficers(newOfficers);
            approvingOffice.UserId = model.UserId;
            approvingOffice.UpdatedAt = System.DateTime.Now;
            await _approverLevelRepo.UpdateApprovingLevelOffice(approvingOffice);
            return CommonResponse.Send(ResponseCodes.SUCCESS, true);
        }
    }
}