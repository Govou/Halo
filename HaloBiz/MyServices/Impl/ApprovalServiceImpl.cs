using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HaloBiz.Adapters;
using HalobizMigrations.Data;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Helpers;
using HalobizMigrations.Models;

using HaloBiz.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using HaloBiz.DTOs.ReceivingDTOs.LAMS;
using HaloBiz.MyServices.LAMS;
using Halobiz.Common.DTOs.ReceivingDTOs;
using Halobiz.Common.Helpers;

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
        private readonly ILeadConversionService _leadConversionService;

        private readonly IMapper _mapper;

        public ApprovalServiceImpl(IModificationHistoryRepository historyRepo, 
            IApprovalRepository approvalRepo,
            IApprovalLimitRepository approvalLimitRepo,
            IProcessesRequiringApprovalRepository processesRequiringApprovalRepo,
            IMailAdapter mailAdapter,
            HalobizContext context,
            ILeadConversionService leadConversionService,
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
            _leadConversionService = leadConversionService;
        }
        public async  Task<ApiCommonResponse> AddApproval(HttpContext context, ApprovalReceivingDTO approvalReceivingDTO)
        {
            var approval = _mapper.Map<Approval>(approvalReceivingDTO);
            approval.CreatedById = context.GetLoggedInUserId();
            var savedApproval = await _approvalRepo.SaveApproval(approval);
            if (savedApproval == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var approvalTransferDTO = _mapper.Map<ApprovalTransferDTO>(approval);
            return CommonResponse.Send(ResponseCodes.SUCCESS,approvalTransferDTO);
        }

        public async Task<ApiCommonResponse> DeleteApproval(long id)
        {
            var approvalToDelete = await _approvalRepo.FindApprovalById(id);
            if(approvalToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            if (!await _approvalRepo.DeleteApproval(approvalToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> GetAllApproval()
        {
            var approval = await _approvalRepo.GetApprovals();
            if (approval == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var approvalTransferDTO = _mapper.Map<IEnumerable<ApprovalTransferDTO>>(approval);
            return CommonResponse.Send(ResponseCodes.SUCCESS,approvalTransferDTO);
        }

        public async Task<ApiCommonResponse> GetPendingApprovals()
        {
            var approval = await _approvalRepo.GetPendingApprovals();
            if (approval == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var approvalTransferDTO = _mapper.Map<IEnumerable<ApprovalTransferDTO>>(approval);
            return CommonResponse.Send(ResponseCodes.SUCCESS,approvalTransferDTO);
        }

        public async Task<ApiCommonResponse> GetPendingApprovalsByQuoteId(long quoteId)
        {
            var approval = await _approvalRepo.GetPendingApprovalsByQuoteId(quoteId);
            if (approval == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var approvalTransferDTO = _mapper.Map<IEnumerable<ApprovalTransferDTO>>(approval);
            return CommonResponse.Send(ResponseCodes.SUCCESS,approvalTransferDTO);
        }
        
        public async Task<ApiCommonResponse> GetApprovalsByQuoteId(long quoteId)
        {
            var approval = await _approvalRepo.GetApprovalsByQuoteId(quoteId);
            if (approval == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var approvalTransferDTO = _mapper.Map<IEnumerable<ApprovalTransferDTO>>(approval);
            return CommonResponse.Send(ResponseCodes.SUCCESS,approvalTransferDTO);
        }

        public async Task<ApiCommonResponse> GetApprovalsByContractId(long contractId)
        {
            var app = await _context.Approvals
               .Where(x => x.ContractId == contractId && x.IsDeleted == false)
               .Include(x => x.Responsible)
               .OrderBy(x => x.Sequence)
               .ToListAsync();

            if (!app.Any())
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }
            var approvalTransferDTO = _mapper.Map<IEnumerable<ApprovalTransferDTO>>(app);
            return CommonResponse.Send(ResponseCodes.SUCCESS, approvalTransferDTO);
        }

        public async Task<ApiCommonResponse> GetPendingApprovalsByContractId(long contractId)
        {
            var app = await _context.Approvals
               .Where(x => x.ContractId == contractId && x.IsApproved==false && x.IsDeleted == false)
               .Include(x => x.Responsible)
               .OrderBy(x => x.Sequence)
               .ToListAsync();

            if (!app.Any())
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }

            var approvalTransferDTO = _mapper.Map<IEnumerable<ApprovalTransferDTO>>(app);
            return CommonResponse.Send(ResponseCodes.SUCCESS, approvalTransferDTO);
        }

        public async Task<ApiCommonResponse> GetPendingApprovalsByServiceId(long serviceId)
        {
            var approval = await _approvalRepo.GetPendingApprovalsByServiceId(serviceId);
            if (approval == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var approvalTransferDTO = _mapper.Map<IEnumerable<ApprovalTransferDTO>>(approval);
            return CommonResponse.Send(ResponseCodes.SUCCESS,approvalTransferDTO);
        }

        public async Task<ApiCommonResponse> GetApprovalsByEndorsementId(long endorsementId)
        {
            var approval = await _approvalRepo.GetApprovalsByEndorsementId(endorsementId);
            if (approval == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var approvalTransferDTO = _mapper.Map<IEnumerable<ApprovalTransferDTO>>(approval);
            return CommonResponse.Send(ResponseCodes.SUCCESS,approvalTransferDTO);
        }

        public async Task<ApiCommonResponse> GetApprovalsByServiceId(long serviceId)
        {
            var approval = await _approvalRepo.GetApprovalsByServiceId(serviceId);
            if (approval == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var approvalTransferDTO = _mapper.Map<IEnumerable<ApprovalTransferDTO>>(approval);
            return CommonResponse.Send(ResponseCodes.SUCCESS,approvalTransferDTO);
        }

        public async Task<ApiCommonResponse> GetUserPendingApprovals(HttpContext context)
        {
            var approval = await _approvalRepo.GetUserPendingApprovals(context.GetLoggedInUserId());
            if (approval == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var approvalTransferDTO = _mapper.Map<IEnumerable<ApprovalTransferDTO>>(approval);
            return CommonResponse.Send(ResponseCodes.SUCCESS,approvalTransferDTO);
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
                                            .Include(x => x.QuoteServices.Where(x => !x.IsDeleted))
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

        public async Task<(bool,string)> SetUpApprovalsForContractCreationEndorsement(long contractId, HttpContext context)
        {
            try
            {
                var contract = await _context.Contracts.AsNoTracking()
                                    .Where(x => x.Id == contractId)
                                    .Include(x=>x.CustomerDivision)
                                    .Include(x => x.ContractServices)
                                        .ThenInclude(x=>x.Branch)
                                    .FirstOrDefaultAsync();
                if (contract == null)
                    return (false,$"No contract with Id {contractId}");

                var module = await _processesRequiringApprovalRepo.FindProcessesRequiringApprovalByCaption("Contract Creation");
                if (module == null)
                {
                    return (false,"No approval module for contract creation");
                }

                var approvalLimits = await _approvalLimitRepo.GetApprovalLimitsByModule(module.Id);
                

                if (!approvalLimits.Any())
                    return (false, $"No approval limit for module with id {module.Id}");


                List<Approval> approvals = new List<Approval>();
                foreach (var conService in contract.ContractServices)
                {
                    conService.Service = await GetServiceInformationForApprovals(conService.ServiceId);
                }

                foreach (var contractService in contract.ContractServices)
                {
                    if (!contractService.BillableAmount.HasValue) continue;

                    var orderedList = approvalLimits
                        .Where(x => contractService.BillableAmount.Value > x.UpperlimitValue ||
                                    (contractService.BillableAmount.Value <= x.UpperlimitValue && contractService.BillableAmount.Value >= x.LowerlimitValue))
                        .OrderBy(x => x.Sequence);

                    foreach (var approvalLimit in orderedList)
                    {
                        long responsibleId = GetWhoIsResponsible(approvalLimit, contractService.Service, contractService.Branch);

                        var approval = new Approval
                        {
                            ContractServiceId = contractService.Id,
                            ContractId = contractService.ContractId,
                            Caption = $"Approval Needed To Create Contract Service {contractService.Service.Name} for client {contract.CustomerDivision.DivisionName}",
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

                if (approvals.Any())
                {
                    var successful = await _approvalRepo.SaveApprovalRange(approvals);
                    if (successful)
                    {
                        await SendMailsForContractApprovals(approvals);
                        return (true,"");
                    }
                    else
                    {
                        return (false,"No approvals set up");
                    }
                }
                return (false, $"No approval setup");
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                _logger.LogInformation(ex.StackTrace);
                return (false, ex.Message);
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
                throw;
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
                return branch?.HeadId ?? throw new Exception($"No head set up for branch head in {branch?.Name}");
            }
            else if (item.ApproverLevel.Caption == "Division Head")
            {
                return service?.Division?.HeadId ?? throw new Exception($"No head set up for division head in {service?.Division?.Name}");
            }
            else if (item.ApproverLevel.Caption == "Operating Entity Head")
            {
                return service?.OperatingEntity?.HeadId ?? throw new Exception($"No head set up for operating entity head in {service?.OperatingEntity?.Name}");
            }
            else if (item.ApproverLevel.Caption == "CEO")
            {
                return service?.Division?.Company?.HeadId ?? throw new Exception($"No head set up for CEO head in {service?.Division?.Company?.Name}");
            }
            else
            {
                throw new Exception($"No approval person set up approval level {item?.ApproverLevel}");
            }
        }

        public async Task<ApiCommonResponse> ApprovalOrDispproveContractService(HttpContext context, ContractApprovalDTO dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var approval = await _context.Approvals.Where(x=>x.Id==dto.approvalId)                  
                    .Include(x=>x.ContractServiceForEndorsement)
                        .ThenInclude(x=>x.EndorsementType)
                    .FirstOrDefaultAsync();
                if (approval == null)
                {
                    return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE, null, "This approval was not found");
                }

                if (approval.IsApproved)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "This has already been approved");
                }

                if (dto.isApproved)
                {
                    approval.IsApproved = true;
                    _context.Approvals.Update(approval);
                    await _context.SaveChangesAsync();

                    //check if this is just endorsement for addition service
                    var endorsementType = approval?.ContractServiceForEndorsement?.EndorsementType;
                    if (endorsementType != null) { 
                        if(endorsementType.Caption.ToLower().Contains("service addition"))
                        {
                            var contractServiceId = (long) approval.ContractServiceForEndorsement.PreviousContractServiceId;
                            var userId = context.GetLoggedInUserId();
                            var endorsement = approval.ContractServiceForEndorsement;

                            //check if is not adhoc invoice
                            if (approval?.ContractService?.InvoicingInterval != (int)TimeCycle.Adhoc)
                            {
                                var contractService = await GetContractService(contractServiceId);
                                if (await ProcessAdditionService(contractService, (long)approval.ContractId, userId, endorsement))
                                {
                                    transaction.Commit();
                                    return CommonResponse.Send(ResponseCodes.REFRESH_APPROVALS, null, "Approval successful and accounts created");
                                }
                                else
                                {
                                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "Could not create accounts after approval");
                                }
                            }
                            else
                            {
                                var contractService = await GetContractService(contractServiceId);

                                contractService.Version = (int)VersionType.Latest;
                                _context.ContractServices.Update(contractService);

                                endorsement.IsApproved = true;
                                endorsement.IsConvertedToContractService = true;
                                _context.ContractServiceForEndorsements.Update(endorsement);
                                await _context.SaveChangesAsync(); return CommonResponse.Send(ResponseCodes.REFRESH_APPROVALS, null, "Adhoc contract service approved");
                            }

                        }
                    }

                    //check if this is the last approval needed
                    var allApprovals = await _context.Approvals.Where(x => x.ContractId == dto.contractId).ToListAsync();
                    if (allApprovals.All(x => x.IsApproved))
                    {
                        //now create accounts etc
                        var contract = await _context.Contracts.AsNoTracking()
                                .Where(x => x.Id == dto.contractId)
                                .Include(x => x.CustomerDivision)
                                .Include(x => x.ContractServices)
                                    .ThenInclude(x => x.Service)
                                 .FirstOrDefaultAsync();

                        var customerDivision = contract.CustomerDivision;
                        long userId = context.GetLoggedInUserId();
                        foreach (var item in contract.ContractServices)
                        {
                            if (item.InvoicingInterval != (int)TimeCycle.Adhoc)
                            {
                                var issuccess = await _leadConversionService.AccountsForContractServices(item, customerDivision, userId);
                                if (!issuccess)
                                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "Could not create accounts after approval");
                            }                           

                        }

                        //update the contract
                        var contractToUpdate = await _context.Contracts.Where(x => x.Id == dto.contractId).FirstOrDefaultAsync();
                        contractToUpdate.IsApproved = true;
                        _context.Update(contractToUpdate); 
                        await _context.SaveChangesAsync();

                        transaction.Commit();
                        return CommonResponse.Send(ResponseCodes.REFRESH_APPROVALS, null, "Approval successful and accounts created");
                    }
                }
                else
                {
                    approval.IsApproved = false;
                    _context.Approvals.Update(approval);

                    await _context.SaveChangesAsync();                    

                    //update the contract with delete
                    var contractToUpdate = await _context.Contracts.Where(x=>x.Id==dto.contractId).FirstOrDefaultAsync();
                    contractToUpdate.IsDeleted = true;
                    _context.Update(contractToUpdate);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ex.Message);
            }

            transaction.Commit();

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        private async Task<ContractService> GetContractService(long previousContractServiceId)
        {
            return await _context.ContractServices.AsNoTracking()
               .Where(x => x.Id == previousContractServiceId)
               .Include(x => x.Service)
               .FirstOrDefaultAsync();
        }

        private async Task<bool> ProcessAdditionService(ContractService contractService, long contractId, long userId, ContractServiceForEndorsement endorsement)
        {
            var contract = await _context.Contracts.AsNoTracking()
                                .Where(x => x.Id == contractId)
                                .Include(x => x.CustomerDivision)
                                 .FirstOrDefaultAsync();

            var customerDivision = contract.CustomerDivision;

            if(await _leadConversionService.AddServiceEndorsement(contractService, customerDivision, userId))
            {
                contractService.Version = (int) VersionType.Latest;
                _context.ContractServices.Update(contractService);

                endorsement.IsApproved = true;
                endorsement.IsConvertedToContractService = true;
                _context.ContractServiceForEndorsements.Update(endorsement);
                await _context.SaveChangesAsync();
                return true;
            }            

            return false;
            
        }
        public  async Task<ApiCommonResponse> UpdateApproval(HttpContext context, long id, ApprovalReceivingDTO approvalReceivingDTO)
        {
            var approvalToUpdate = await _approvalRepo.FindApprovalById(id);
            if (approvalToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            
            var summary = $"Initial details before change, \n {approvalToUpdate} \n" ;

            approvalToUpdate.Caption = approvalReceivingDTO.Caption;
            approvalToUpdate.Sequence = approvalReceivingDTO.Sequence;
            var updatedApproval = await _approvalRepo.UpdateApproval(approvalToUpdate);

            summary += $"Details after change, \n {updatedApproval.ToString()} \n";

            if (updatedApproval == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "Approval",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedApproval.Id
            };

            await _historyRepo.SaveHistory(history);

            var approvalTransferDTOs = _mapper.Map<ApprovalTransferDTO>(updatedApproval);
            return CommonResponse.Send(ResponseCodes.SUCCESS,approvalTransferDTOs);
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

        public async Task<ApiCommonResponse> GetApprovingLevelOfficeData(HttpContext context)
        {
            try
            {
                var userProfileId = context.GetLoggedInUserId();

                var allApprovingLevelOffices = await _context.ApprovingLevelOffices
                    .Include(x => x.ApproverLevel)
                    .Where(x => x.IsDeleted == false)
                    .ToListAsync();

                var officerProfiles = await _context.ApprovingLevelOfficers
                    .Include(x => x.ApprovingLevelOffice)
                    .Where(x => x.UserId == userProfileId && x.IsDeleted == false)
                    .ToListAsync();

                ApprovingLevelBackUp resultObject = new()
                {
                    CurrentUserId = userProfileId,
                    BackUpInfos = new List<ApprovingLevelBackUpInfo>()
                };

                foreach (var office in allApprovingLevelOffices)
                {
                    ApprovingLevelBackUpInfo info = new();
                    info.Level = office.ApproverLevel.Caption;
                    info.CanApprove = officerProfiles.Any(x => x.ApprovingLevelOfficeId == office.Id);
                    resultObject.BackUpInfos.Add(info);
                }

                return CommonResponse.Send(ResponseCodes.SUCCESS, resultObject);
            }
            catch(Exception error)
            {
                _logger.LogError(error.Message);
                return CommonResponse.Send(ResponseCodes.FAILURE, error.Message);
            }
        }
    }
}

