using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HaloBiz.Adapters;
using HalobizMigrations.Data;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Helpers;
using HalobizMigrations.Models;

using HaloBiz.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace HaloBiz.MyServices.Impl
{
    public class ApprovalServiceImpl : IApprovalService
    {
        private readonly HalobizContext _context;
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
            HalobizContext context,
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

        public async Task<ApiResponse> GetApprovalsByEndorsementId(long endorsementId)
        {
            var approval = await _approvalRepo.GetApprovalsByEndorsementId(endorsementId);
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
                        quoteService.Service = await GetServiceInformationForApprovals(quoteService.ServiceId);
                    }

                    foreach (var quoteService in quote.QuoteServices)
                    {
                        if (!quoteService.BillableAmount.HasValue) continue;

                        var orderedList = approvalLimits
                            .Where(x => quoteService.BillableAmount.Value > x.UpperlimitValue || 
                                        (quoteService.BillableAmount.Value <= x.UpperlimitValue && quoteService.BillableAmount.Value >= x.LowerlimitValue))
                            .OrderBy(x => x.Sequence);
                        
                        foreach (var approvalLimit in orderedList)
                        {
                            long responsibleId = GetWhoIsResponsible(approvalLimit, quoteService.Service, leadDivision.Branch);

                            var approval = new Approval
                            {
                                QuoteServiceId = quoteService.Id,
                                QuoteId = quote.Id,
                                Caption = $"Approval Needed To Create Contract Service {quoteService.Service.Name} for Client {leadDivision.DivisionName} under {lead.GroupName}",
                                CreatedById = context.GetLoggedInUserId(),
                                Sequence = approvalLimit.Sequence,
                                ResponsibleId = responsibleId,
                                IsApproved = false,
                                DateTimeApproved = null,
                                Level = approvalLimit.ApproverLevel.Caption
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

        public async Task<bool> SetUpApprovalsForContractModificationEndorsement(ContractServiceForEndorsement contractServiceForEndorsement, HttpContext context)
        {
            try
            {
                var endorsementType = await _context.EndorsementTypes.SingleOrDefaultAsync(x => x.Id == contractServiceForEndorsement.EndorsementTypeId);
                if(endorsementType == null)
                {
                    return false;
                }

                var module = await _processesRequiringApprovalRepo.FindProcessesRequiringApprovalByCaption(endorsementType.Caption);
                if (module == null)
                {
                    return false;
                }

                var approvalLimits = await _approvalLimitRepo.GetApprovalLimitsByModule(module.Id);
                if(approvalLimits == null)
                {
                    return false;
                }

                if (!approvalLimits.Any()) return false;

                List<Approval> approvals = new List<Approval>();

                contractServiceForEndorsement.Service = await GetServiceInformationForApprovals(contractServiceForEndorsement.ServiceId);
                contractServiceForEndorsement.Branch = await _context.Branches.FindAsync(contractServiceForEndorsement.BranchId);

                if (!contractServiceForEndorsement.BillableAmount.HasValue) return false;

                double amountChangeValue = contractServiceForEndorsement.BillableAmount.Value;

                var edType = endorsementType.Caption.ToLower();
                if (edType.Contains("topup") || edType.Contains("reduction"))
                {
                    var prevContractService = await _context.ContractServices.AsNoTracking()
                                                    .Where(x => x.Id == contractServiceForEndorsement.PreviousContractServiceId)
                                                    .SingleOrDefaultAsync();

                    amountChangeValue = Math.Abs(contractServiceForEndorsement.BillableAmount.Value - prevContractService.BillableAmount.Value);
                }

                var orderedList = approvalLimits
                    .Where(x => amountChangeValue > x.UpperlimitValue ||
                                (amountChangeValue <= x.UpperlimitValue &&
                                amountChangeValue >= x.LowerlimitValue))
                    .OrderBy(x => x.Sequence);

                var customerDivision = await _context.CustomerDivisions.Where(x => x.Id == contractServiceForEndorsement.CustomerDivisionId)
                    .Include(x => x.Customer)
                    .FirstOrDefaultAsync();

                foreach (var item in orderedList)
                {
                    var approvalLevelInfo = item.ApproverLevel;

                    long responsibleId = GetWhoIsResponsible(item, contractServiceForEndorsement.Service, contractServiceForEndorsement.Branch);

                    var approval = new Approval
                    {
                        ContractId = contractServiceForEndorsement.ContractId,
                        ContractServiceForEndorsementId = contractServiceForEndorsement.Id,
                        Caption = $"Approval needed to endorse {endorsementType.Caption} for contract service {contractServiceForEndorsement.Service.Name} for client {customerDivision.DivisionName} under {customerDivision.Customer.GroupName}",
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
        
        public async Task<bool> SetUpApprovalsForContractRenewalEndorsement(List<ContractServiceForEndorsement> contractServiceForEndorsements, HttpContext context)
        {
            try
            {
                var module = await _processesRequiringApprovalRepo.FindProcessesRequiringApprovalByCaption("Service Retention");
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

                List<Approval> approvals = new List<Approval>();

                foreach (var contractServiceForEndorsement in contractServiceForEndorsements)
                {
                    contractServiceForEndorsement.Service = await GetServiceInformationForApprovals(contractServiceForEndorsement.ServiceId);
                    contractServiceForEndorsement.Branch = await _context.Branches.FindAsync(contractServiceForEndorsement.BranchId);
                }

                foreach (var contractServiceForEndorsement in contractServiceForEndorsements)
                {
                    if (!contractServiceForEndorsement.BillableAmount.HasValue) return false;

                    var orderedList = approvalLimits
                        .Where(x => contractServiceForEndorsement.BillableAmount.Value > x.UpperlimitValue ||
                                    (contractServiceForEndorsement.BillableAmount.Value <= x.UpperlimitValue && 
                                    contractServiceForEndorsement.BillableAmount.Value >= x.LowerlimitValue))
                        .OrderBy(x => x.Sequence);

                    var customerDivision = await _context.CustomerDivisions.Where(x => x.Id == contractServiceForEndorsement.CustomerDivisionId)
                        .Include(x => x.Customer)
                        .FirstOrDefaultAsync();

                    foreach (var item in orderedList)
                    {
                        var approvalLevelInfo = item.ApproverLevel;

                        long responsibleId = GetWhoIsResponsible(item, contractServiceForEndorsement.Service, contractServiceForEndorsement.Branch);                      

                        var approval = new Approval
                        {
                            ContractId = contractServiceForEndorsement.ContractId,
                            ContractServiceForEndorsementId = contractServiceForEndorsement.Id,
                            Caption = $"Approval Needed To Renew Contract Service {contractServiceForEndorsement.Service.Name} for Client {customerDivision.DivisionName} under {customerDivision.Customer.GroupName}",
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

                if (approvals.Any())
                {
                    var successful = await _approvalRepo.SaveApprovalRange(approvals);
                    if (successful)
                    {
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
        
        public async Task<bool> SetUpApprovalsForServiceCreation(Service service, HttpContext context)
        {
            try
            {
                var serviceWithExtraInfo = await GetServiceInformationForApprovals(service.Id);
                
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

                foreach (var approvalLimit in orderedList)
                {
                    // How to tell branch head ?? skip.
                    if (approvalLimit.ApproverLevel.Caption == "Branch Head") continue;

                    long responsibleId = GetWhoIsResponsible(approvalLimit, serviceWithExtraInfo, null);

                    var approval = new Approval
                    {
                        ServicesId = service.Id,
                        Caption = $"Approval Needed To Create Service {service.Name}",
                        CreatedById = context.GetLoggedInUserId(),
                        Sequence = approvalLimit.Sequence,
                        ResponsibleId = responsibleId,
                        IsApproved = false,
                        DateTimeApproved = null,
                        Level = approvalLimit.ApproverLevel.Caption                         
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

        private async Task<Service> GetServiceInformationForApprovals(long servicesId)
        {
            var services = await _context.Services.AsNoTracking().Where(x => x.Id == servicesId)
                    .Include(x => x.Division)
                    .ThenInclude(x => x.Company)
                    .Include(x => x.OperatingEntity)
                    .ThenInclude(x => x.Head)
                    .FirstOrDefaultAsync();

            return services;
        }

        private long GetWhoIsResponsible(ApprovalLimit item, Service service, Branch branch)
        {
            if (item.ApproverLevel.Caption == "Branch Head")
            {
                return branch?.HeadId ?? 1;                          
            }
            else if (item.ApproverLevel.Caption == "Division Head")
            {
                return service?.Division?.HeadId ?? 1;
            }
            else if (item.ApproverLevel.Caption == "Operating Entity Head")
            {
                return service?.OperatingEntity?.HeadId ?? 1;
            }
            else if (item.ApproverLevel.Caption == "CEO")
            {
                return service?.Division?.Company?.HeadId.Value ?? 1;
            }
            else
            {
                return 1;
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