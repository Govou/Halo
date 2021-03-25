using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HaloBiz.Adapters;
using HaloBiz.Data;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Helpers;
using HaloBiz.Model;
using HaloBiz.Model.LAMS;
using HaloBiz.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace HaloBiz.MyServices.Impl
{
    public class ApprovalServiceImpl : IApprovalService
    {
        private readonly DataContext _context;
        private readonly ILogger<ApprovalServiceImpl> _logger;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly IApprovalRepository _approvalRepo;
        private readonly IApprovalLimitRepository _approvalLimitRepo;
        private readonly IProcessesRequiringApprovalRepository _processesRequiringApprovalRepo;
        private readonly IMailAdapter _mailAdapter;

        private readonly IMapper _mapper;

        public ApprovalServiceImpl(IModificationHistoryRepository historyRepo, 
            IApprovalRepository approvalRepo,
            IApprovalLimitRepository approvalLimitRepo,
            IProcessesRequiringApprovalRepository processesRequiringApprovalRepo,
            IMailAdapter mailAdapter,
            DataContext context,
            ILogger<ApprovalServiceImpl> logger, IMapper mapper)
        {
            _mapper = mapper;
            _historyRepo = historyRepo;
            _approvalRepo = approvalRepo;
            _approvalLimitRepo = approvalLimitRepo;
            _processesRequiringApprovalRepo = processesRequiringApprovalRepo;
            _mailAdapter = mailAdapter;
            _context = context;
            _logger = logger;
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

        public async Task<ApiResponse> GetPendingApprovalsByQuoteId(long quoteId)
        {
            var approval = await _approvalRepo.GetPendingApprovalsByQuoteId(quoteId);
            if (approval == null)
            {
                return new ApiResponse(404);
            }
            var approvalTransferDTO = _mapper.Map<IEnumerable<ApprovalTransferDTO>>(approval);
            return new ApiOkResponse(approvalTransferDTO);
        }
        
        public async Task<ApiResponse> GetApprovalsByQuoteId(long quoteId)
        {
            var approval = await _approvalRepo.GetApprovalsByQuoteId(quoteId);
            if (approval == null)
            {
                return new ApiResponse(404);
            }
            var approvalTransferDTO = _mapper.Map<IEnumerable<ApprovalTransferDTO>>(approval);
            return new ApiOkResponse(approvalTransferDTO);
        }

        public async Task<ApiResponse> GetPendingApprovalsByServiceId(long serviceId)
        {
            var approval = await _approvalRepo.GetPendingApprovalsByServiceId(serviceId);
            if (approval == null)
            {
                return new ApiResponse(404);
            }
            var approvalTransferDTO = _mapper.Map<IEnumerable<ApprovalTransferDTO>>(approval);
            return new ApiOkResponse(approvalTransferDTO);
        }
        
        public async Task<ApiResponse> GetApprovalsByServiceId(long serviceId)
        {
            var approval = await _approvalRepo.GetApprovalsByServiceId(serviceId);
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

        public async Task<bool> SetUpApprovalsForClientCreation(long leadId, HttpContext context)
        {
            try
            {
                var module = await _processesRequiringApprovalRepo.FindProcessesRequiringApprovalByCaption("Contract Creation");
                if (module == null)
                {
                    return false;
                }

                var approvalLimits = await _approvalLimitRepo.GetApprovalLimitsByModule(module.Id);
                if (approvalLimits == null)
                {
                    return false;
                }

                if (!approvalLimits.Any()) return false;

                var lead = await _context.Leads.AsNoTracking()
                        .Include(x => x.LeadDivisions)
                           .ThenInclude(x => x.Branch)
                        .FirstOrDefaultAsync(x => x.Id == leadId);

                List<Approval> approvals = new List<Approval>();
                foreach (var leadDivision in lead.LeadDivisions)
                {
                    var quote = await _context.Quotes.AsNoTracking()
                                            .Include(x => x.QuoteServices)
                                            .FirstOrDefaultAsync(x => x.LeadDivisionId == leadDivision.Id);

                    if(quote == null)
                    {
                        return false;
                    }
                    
                    foreach (var quoteService in quote.QuoteServices)
                    {
                        quoteService.Service = await _context.Services.AsNoTracking().Where(x => x.Id == quoteService.ServiceId)
                            .Include(x => x.Division)
                            .ThenInclude(x => x.Company)
                            .Include(x => x.OperatingEntity)
                            .ThenInclude(x => x.Head)
                            .FirstOrDefaultAsync();
                    }

                    foreach (var quoteService in quote.QuoteServices)
                    {
                        if (!quoteService.BillableAmount.HasValue) continue;

                        var orderedList = approvalLimits
                            .Where(x => quoteService.BillableAmount.Value > x.UpperlimitValue || 
                                        (quoteService.BillableAmount.Value <= x.UpperlimitValue && quoteService.BillableAmount.Value >= x.LowerlimitValue))
                            .OrderBy(x => x.Sequence);
                        
                        foreach (var item in orderedList)
                        {
                            var approvalLevelInfo = item.ApproverLevel;

                            long responsibleId = 0;
                            if (item.ApproverLevel.Caption == "Branch Head")
                            {
                                responsibleId = leadDivision.Branch?.HeadId ?? 31;
                            }
                            else if (item.ApproverLevel.Caption == "Division Head")
                            {
                                responsibleId = quoteService.Service?.Division?.HeadId ?? 31;
                            }
                            else if (item.ApproverLevel.Caption == "Operating Entity Head")
                            {
                                responsibleId = quoteService.Service?.OperatingEntity?.HeadId ?? 31;
                            }
                            else if (item.ApproverLevel.Caption == "CEO")
                            {
                                responsibleId = quoteService.Service?.Division?.Company?.HeadId.Value ?? 31;
                            }

                            var approval = new Approval
                            {
                                QuoteServiceId = quoteService.Id,
                                QuoteId = quote.Id,
                                Caption = $"Approval Needed To Create Contract Service {quoteService.Service.Name} for Client {leadDivision.DivisionName} under {lead.GroupName}",
                                CreatedById = context.GetLoggedInUserId(),
                                Sequence = item.Sequence,
                                ResponsibleId = responsibleId,
                                IsApproved = false,
                                DateTimeApproved = null,
                                Level = item.ApproverLevel.Caption
                            };

                            approvals.Add(approval);
                        }
                    }                      
                }
                if (approvals.Any())
                {
                    var successful = await _approvalRepo.SaveApprovalRange(approvals);
                    if (successful)
                    {
                        await SendMailsForContractApprovals(approvals);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                _logger.LogInformation(ex.StackTrace);
                return false;
            }
        }

        public async Task<bool> SetUpApprovalsForServiceTopupEndorsement(CustomerDivision customerDivision, HttpContext httpContext)
        {
            try
            {
                var module = await _processesRequiringApprovalRepo.FindProcessesRequiringApprovalByCaption("Endorsement:Service Topup");
                if (module == null)
                {
                    return false;
                }
                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                _logger.LogInformation(ex.StackTrace);
                return false;
            }
        }
        
        public async Task<bool> SetUpApprovalsForServiceReductionEndorsement(CustomerDivision customerDivision, HttpContext httpContext)
        {
            try
            {
                var module = await _processesRequiringApprovalRepo.FindProcessesRequiringApprovalByCaption("Endorsement:Service Reduction");
                if (module == null)
                {
                    return false;
                }
                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                _logger.LogInformation(ex.StackTrace);
                return false;
            }
        }
        
        public async Task<bool> SetUpApprovalsForServiceAdditionEndorsement(CustomerDivision customerDivision, HttpContext httpContext)
        {
            try
            {
                var module = await _processesRequiringApprovalRepo.FindProcessesRequiringApprovalByCaption("Endorsement:Service Addition");
                if (module == null)
                {
                    return false;
                }
                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                _logger.LogInformation(ex.StackTrace);
                return false;
            }
        }
        
        public async Task<bool> SetUpApprovalsForServiceRenewalEndorsement(CustomerDivision customerDivision, HttpContext httpContext)
        {
            try
            {
                var module = await _processesRequiringApprovalRepo.FindProcessesRequiringApprovalByCaption("Endorsement:Service Renewal");
                if (module == null)
                {
                    return false;
                }
                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                _logger.LogInformation(ex.StackTrace);
                return false;
            }
        }
        
        public async Task<bool> SetUpApprovalsForServiceCreation(Services service, HttpContext context)
        {
            try
            {
                var serviceWithExtraInfo = await _context.Services.AsNoTracking().Where(x => x.Id == service.Id)
                    .Include(x => x.Division)
                    .ThenInclude(x => x.Company)
                    .Include(x => x.OperatingEntity)
                    .ThenInclude(x => x.Head)
                    .FirstOrDefaultAsync();
                
                var module = await _processesRequiringApprovalRepo.FindProcessesRequiringApprovalByCaption("Service Creation");
                if (module == null)
                {
                    return false;
                }

                var approvalLimits = await _approvalLimitRepo.GetApprovalLimitsByModule(module.Id);

                if (!approvalLimits.Any()) return false;

                var orderedList = approvalLimits
                    .Where(x => service.UnitPrice < x.UpperlimitValue || (service.UnitPrice <= x.UpperlimitValue && service.UnitPrice >= x.LowerlimitValue))
                    .OrderBy(x => x.Sequence);

                List<Approval> approvals = new List<Approval>();

                foreach (var item in orderedList)
                {
                    long responsibleId = 0;
                    
                    // How to tell branch head ??
                    if (item.ApproverLevel.Caption == "Branch Head")
                    {
                        continue;
                    }
                    else if (item.ApproverLevel.Caption == "Division Head")
                    {
                        responsibleId = serviceWithExtraInfo?.Division?.HeadId ?? 31;
                    }
                    else if (item.ApproverLevel.Caption == "Operating Entity Head")
                    {
                        responsibleId = serviceWithExtraInfo?.OperatingEntity?.HeadId ?? 31;
                    }
                    else if (item.ApproverLevel.Caption == "CEO")
                    {
                        responsibleId = serviceWithExtraInfo?.Division?.Company?.HeadId.Value ?? 31;
                    }

                    var approval = new Approval
                    {
                        ServicesId = service.Id,
                        Caption = $"Approval Needed To Create Service {service.Name}",
                        CreatedById = context.GetLoggedInUserId(),
                        Sequence = item.Sequence,
                        ResponsibleId = responsibleId,
                        IsApproved = false,
                        DateTimeApproved = null,
                        Level = item.ApproverLevel.Caption                         
                    };

                    approvals.Add(approval);
                }

                if (approvals.Any())
                {
                    var successful = await _approvalRepo.SaveApprovalRange(approvals);
                    if (successful)
                    {
                        await SendMailsForServiceApprovals(approvals);                      
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
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
            
            var summary = $"Initial details before change, \n {approvalToUpdate} \n" ;

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

        private async Task SendMailsForServiceApprovals(List<Approval> approvals)
        {
            foreach (var approval in approvals)
            {
                approval.Responsible = await _context.UserProfiles.FindAsync(approval.ResponsibleId);

                approval.Services = await _context.Services.AsNoTracking()
                    .Where(x => x.Id == approval.ServicesId)
                    .Include(x => x.OperatingEntity)
                    .Include(x => x.Division)
                    .Include(x => x.ServiceCategory)
                    .FirstOrDefaultAsync();

                var serializedApproval = JsonConvert.SerializeObject(approval, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

                Action action = async () => {
                    await _mailAdapter.ApproveNewService(serializedApproval);
                };

                action.RunAsTask();
            }
        }

        private async Task SendMailsForContractApprovals(List<Approval> approvals)
        {
            foreach (var approval in approvals)
            {
                approval.Responsible = await _context.UserProfiles.FindAsync(approval.ResponsibleId);

                approval.QuoteService = await _context.QuoteServices.AsNoTracking()
                    .Where(x => x.Id == approval.QuoteServiceId)
                    .Include(x => x.Service).ThenInclude(x => x.OperatingEntity)
                    .Include(x => x.Service).ThenInclude(x => x.ServiceCategory)
                    .Include(x => x.Service).ThenInclude(x => x.Division)
                    .FirstOrDefaultAsync();

                var serializedApproval = JsonConvert.SerializeObject(approval, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

                Action action = async () => {
                    await _mailAdapter.ApproveNewQuoteService(serializedApproval);
                };

                action.RunAsTask();
            }
        }
    }
}