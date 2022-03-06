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

namespace HaloBiz.MyServices.Impl
{
    public class ApprovalLimitServiceImpl : IApprovalLimitService
    {
        private readonly ILogger<ApprovalLimitServiceImpl> _logger;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly IApprovalLimitRepository _approvalLimitRepo;
        private readonly IMapper _mapper;

        public ApprovalLimitServiceImpl(IModificationHistoryRepository historyRepo, IApprovalLimitRepository approvalLimitRepo, ILogger<ApprovalLimitServiceImpl> logger, IMapper mapper)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._approvalLimitRepo = approvalLimitRepo;
            this._logger = logger;
        }
        public async  Task<ApiCommonResponse> AddApprovalLimit(HttpContext context, ApprovalLimitReceivingDTO approvalLimitReceivingDTO)
        {
            var approvalLimit = _mapper.Map<ApprovalLimit>(approvalLimitReceivingDTO);
            approvalLimit.CreatedById = context.GetLoggedInUserId();
            var savedApprovalLimit = await _approvalLimitRepo.SaveApprovalLimit(approvalLimit);
            if (savedApprovalLimit == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var approvalLimitTransferDTO = _mapper.Map<ApprovalLimitTransferDTO>(approvalLimit);
            return CommonResponse.Send(ResponseCodes.SUCCESS,approvalLimitTransferDTO);
        }

        public async Task<ApiCommonResponse> DeleteApprovalLimit(long id)
        {
            var approvalLimitToDelete = await _approvalLimitRepo.FindApprovalLimitById(id);
            if(approvalLimitToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            if (!await _approvalLimitRepo.DeleteApprovalLimit(approvalLimitToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> GetAllApprovalLimit()
        {
            var approvalLimit = await _approvalLimitRepo.GetApprovalLimits();
            if (approvalLimit == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var approvalLimitTransferDTO = _mapper.Map<IEnumerable<ApprovalLimitTransferDTO>>(approvalLimit);
            return CommonResponse.Send(ResponseCodes.SUCCESS,approvalLimitTransferDTO);
        }

        public  async Task<ApiCommonResponse> UpdateApprovalLimit(HttpContext context, long id, ApprovalLimitReceivingDTO approvalLimitReceivingDTO)
        {
            var approvalLimitToUpdate = await _approvalLimitRepo.FindApprovalLimitById(id);
            if (approvalLimitToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            
            var summary = $"Initial details before change, \n {approvalLimitToUpdate.ToString()} \n" ;

            approvalLimitToUpdate.Caption = approvalLimitReceivingDTO.Caption;
            approvalLimitToUpdate.Description = approvalLimitReceivingDTO.Description;
            approvalLimitToUpdate.ProcessesRequiringApprovalId = approvalLimitReceivingDTO.ProcessesRequiringApprovalId;
            approvalLimitToUpdate.UpperlimitValue = approvalLimitReceivingDTO.UpperlimitValue;
            approvalLimitToUpdate.LowerlimitValue = approvalLimitReceivingDTO.LowerlimitValue;
            approvalLimitToUpdate.ApproverLevelId = approvalLimitReceivingDTO.ApproverLevelId;
            approvalLimitToUpdate.Sequence = approvalLimitReceivingDTO.Sequence;
            approvalLimitToUpdate.IsBypassRequired = approvalLimitReceivingDTO.IsBypassRequired;
            var updatedApprovalLimit = await _approvalLimitRepo.UpdateApprovalLimit(approvalLimitToUpdate);

            summary += $"Details after change, \n {updatedApprovalLimit.ToString()} \n";

            if (updatedApprovalLimit == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "ApprovalLimit",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedApprovalLimit.Id
            };

            await _historyRepo.SaveHistory(history);

            var approvalLimitTransferDTOs = _mapper.Map<ApprovalLimitTransferDTO>(updatedApprovalLimit);
            return CommonResponse.Send(ResponseCodes.SUCCESS,approvalLimitTransferDTOs);
        }
    }
}