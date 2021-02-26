using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Helpers;
using HaloBiz.Model;
using HaloBiz.Model.LAMS;
using HaloBiz.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace HaloBiz.MyServices.Impl
{
    public class ApprovalServiceImpl : IApprovalService
    {
        private readonly ILogger<ApprovalServiceImpl> _logger;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly IApprovalRepository _approvalRepo;
        private readonly IApprovalLimitRepository _approvalLimitRepo;
        private readonly IProcessesRequiringApprovalRepository _processesRequiringApprovalRepo;

        private readonly IMapper _mapper;

        public ApprovalServiceImpl(IModificationHistoryRepository historyRepo, 
            IApprovalRepository approvalRepo,
            IApprovalLimitRepository approvalLimitRepo,
            IProcessesRequiringApprovalRepository processesRequiringApprovalRepo,
            ILogger<ApprovalServiceImpl> logger, IMapper mapper)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._approvalRepo = approvalRepo;
            _approvalLimitRepo = approvalLimitRepo;
            _processesRequiringApprovalRepo = processesRequiringApprovalRepo;
            this._logger = logger;
        }
        public async  Task<ApiResponse> AddApproval(HttpContext context, ApprovalReceivingDTO approvalReceivingDTO)
        {
            var approval = _mapper.Map<Approval>(approvalReceivingDTO);
            approval.CreatedById = context.GetLoggedInUserId();
            var savedApproval = await _approvalRepo.SaveApproval(approval);
            if (savedApproval == null)
            {
                return new ApiResponse(500);
            }
            var approvalTransferDTO = _mapper.Map<ApprovalTransferDTO>(approval);
            return new ApiOkResponse(approvalTransferDTO);
        }

        public async Task<ApiResponse> DeleteApproval(long id)
        {
            var approvalToDelete = await _approvalRepo.FindApprovalById(id);
            if(approvalToDelete == null)
            {
                return new ApiResponse(404);
            }
            if (!await _approvalRepo.DeleteApproval(approvalToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> GetAllApproval()
        {
            var approval = await _approvalRepo.GetApprovals();
            if (approval == null)
            {
                return new ApiResponse(404);
            }
            var approvalTransferDTO = _mapper.Map<IEnumerable<ApprovalTransferDTO>>(approval);
            return new ApiOkResponse(approvalTransferDTO);
        }

        public async Task<ApiResponse> GetPendingApprovals()
        {
            var approval = await _approvalRepo.GetPendingApprovals();
            if (approval == null)
            {
                return new ApiResponse(404);
            }
            var approvalTransferDTO = _mapper.Map<IEnumerable<ApprovalTransferDTO>>(approval);
            return new ApiOkResponse(approvalTransferDTO);
        }

        public async Task<ApiResponse> GetUserPendingApprovals(HttpContext context)
        {
            var approval = await _approvalRepo.GetUserPendingApprovals(context.GetLoggedInUserId());
            if (approval == null)
            {
                return new ApiResponse(404);
            }
            var approvalTransferDTO = _mapper.Map<IEnumerable<ApprovalTransferDTO>>(approval);
            return new ApiOkResponse(approvalTransferDTO);
        }

        public async Task<bool> SetUpApprovalsForClientCreation(Lead lead, HttpContext httpContext)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SetUpApprovalsForEndorsement(CustomerDivision customerDivision, HttpContext httpContext)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SetUpApprovalsForServiceCreation(Services service, HttpContext context)
        {
            try
            {
                var module = await _processesRequiringApprovalRepo.FindProcessesRequiringApprovalByCaption("Service Creation");
                if (module == null)
                {
                    return false;
                }
                var approvalLimits = await _approvalLimitRepo.GetApprovalLimitsByModule(module.Id);
                var orderedList = approvalLimits
                    .Where(x => service.UnitPrice > x.UpperlimitValue || (service.UnitPrice <= x.UpperlimitValue && service.UnitPrice >= x.LowerlimitValue))
                    .OrderBy(x => x.Sequence);

                List<Approval> approvals = new List<Approval>();

                foreach (var item in orderedList)
                {
                    var approvalLevelInfo = item.ApproverLevel;

                    long responsibleId = 0;
                    if (item.ApproverLevel.Caption == "Division Head")
                    {
                        responsibleId = service.Division.HeadId;
                    }
                    else if (item.ApproverLevel.Caption == "Operating Entity Head")
                    {
                        responsibleId = service.OperatingEntity.HeadId;
                    }
                    else if (item.ApproverLevel.Caption == "CEO")
                    {
                        responsibleId = service.Division.Company?.HeadId.Value ?? 31;
                    }

                    var approval = new Approval
                    {
                        ServicesId = service.Id,
                        Caption = $"Approval Needed To Create Service {service.Name}",
                        CreatedById = context.GetLoggedInUserId(),
                        Sequence = item.Sequence,
                        ResponsibleId = responsibleId,
                        IsApproved = false,
                    };

                    approvals.Add(approval);
                }

                if (approvals.Any())
                {
                    return await _approvalRepo.SaveApprovalRange(approvals);
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                _logger.LogInformation(ex.StackTrace);
                return false;
            }
        }

        public  async Task<ApiResponse> UpdateApproval(HttpContext context, long id, ApprovalReceivingDTO approvalReceivingDTO)
        {
            var approvalToUpdate = await _approvalRepo.FindApprovalById(id);
            if (approvalToUpdate == null)
            {
                return new ApiResponse(404);
            }
            
            var summary = $"Initial details before change, \n {approvalToUpdate.ToString()} \n" ;

            approvalToUpdate.Caption = approvalReceivingDTO.Caption;
            approvalToUpdate.Sequence = approvalReceivingDTO.Sequence;
            var updatedApproval = await _approvalRepo.UpdateApproval(approvalToUpdate);

            summary += $"Details after change, \n {updatedApproval.ToString()} \n";

            if (updatedApproval == null)
            {
                return new ApiResponse(500);
            }
            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "Approval",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedApproval.Id
            };

            await _historyRepo.SaveHistory(history);

            var approvalTransferDTOs = _mapper.Map<ApprovalTransferDTO>(updatedApproval);
            return new ApiOkResponse(approvalTransferDTOs);
        }
    }
}