using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HaloBiz.Data;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs.LAMS;
using HaloBiz.DTOs.TransferDTOs.LAMS;
using HaloBiz.Helpers;
using HaloBiz.Model;
using HaloBiz.Model.AccountsModel;
using HaloBiz.Model.LAMS;
using HaloBiz.MyServices.LAMS;
using HaloBiz.Repository;
using HaloBiz.Repository.Impl;
using HaloBiz.Repository.LAMS;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace HaloBiz.MyServices.Impl.LAMS
{
    public class LeadServiceImpl : ILeadService
    {
        private readonly ILogger<LeadServiceImpl> _logger;
        private readonly IReferenceNumberRepository _refNumberRepo;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly ILeadRepository _leadRepo;
        private readonly IMapper _mapper;
        private readonly ILeadConversionService _leadConversionService;
        private readonly IApprovalService _approvalService;
        private readonly IModificationHistoryRepository _modificationRepo;
        private readonly DataContext _context;

        public long LoggedInUserId { get; set; }

        public LeadServiceImpl(
                                IReferenceNumberRepository refNumberRepo,
                                IModificationHistoryRepository historyRepo,
                                ILeadRepository leadRepo,
                                ILogger<LeadServiceImpl> logger,
                                IMapper mapper,
                                ILeadConversionService leadConversionService,
                                IApprovalService approvalService,
                                IModificationHistoryRepository modificationHistoryRepo,
                                DataContext context
                                )
        {
            _mapper = mapper;
            _leadConversionService = leadConversionService;
            _refNumberRepo = refNumberRepo;
            _historyRepo = historyRepo;
            _leadRepo = leadRepo;
            _approvalService = approvalService;
            _modificationRepo = modificationHistoryRepo;
            _context = context;
            _logger = logger;
        }

        public async Task<ApiResponse> AddLead(HttpContext context, LeadReceivingDTO leadReceivingDTO)
        {
            var lead = _mapper.Map<Lead>(leadReceivingDTO);
            lead.CreatedById = context.GetLoggedInUserId();
            var referenceNumber = await _refNumberRepo.GetReferenceNumber();
            if(referenceNumber == null)
            {
                return new ApiResponse(500);
            }

            lead.ReferenceNo = referenceNumber.ReferenceNo.GenerateReferenceNumber();
            referenceNumber.ReferenceNo = referenceNumber.ReferenceNo + 1;
            var isUpdatedRefNumber = await _refNumberRepo.UpdateReferenceNumber(referenceNumber);

            if(!isUpdatedRefNumber)
            {
                return new ApiResponse(500);
            }

            var savedLead = await _leadRepo.SaveLead(lead);
            if (savedLead == null)
            {
                return new ApiResponse(500);
            }
            var leadTransferDTO = _mapper.Map<LeadTransferDTO>(savedLead);
            return new ApiOkResponse(leadTransferDTO);
        }

        public async Task<ApiResponse> GetAllLead()
        {
            var leads = await _leadRepo.FindAllLead();
            if (leads == null)
            {
                return new ApiResponse(404);
            }
            var leadTransferDTO = _mapper.Map<IEnumerable<LeadTransferDTO>>(leads);
            return new ApiOkResponse(leadTransferDTO);
        }

        public async Task<ApiResponse> GetAllUnApprovedLeads()
        {
            var leads = await _leadRepo.FindAllUnApprovedLeads();
            if (leads == null)
            {
                return new ApiResponse(404);
            }
            var leadTransferDTO = _mapper.Map<IEnumerable<LeadTransferDTO>>(leads);
            return new ApiOkResponse(leadTransferDTO);
        }

        public async Task<ApiResponse> SetUpLeadForApproval(HttpContext httpContext, long id)
        {
            var approvalsCreatedSuccessfully = await _approvalService.SetUpApprovalsForClientCreation(id, httpContext);
                    
            if(approvalsCreatedSuccessfully)
            {
                return new ApiOkResponse(true);
            }
            else
            {
                return new ApiResponse(500);
            }
        }

        public async Task<ApiResponse> GetLeadById(long id)
        {
            var lead = await _leadRepo.FindLeadById(id);
            if (lead == null)
            {
                return new ApiResponse(404);
            }
            var leadTransferDTOs = _mapper.Map<LeadTransferDTO>(lead);
            return new ApiOkResponse(leadTransferDTOs);
        }

        public async Task<ApiResponse> GetLeadByReferenceNumber(string refNumber)
        {
            var lead = await _leadRepo.FindLeadByReferenceNo(refNumber);
            if (lead == null)
            {
                return new ApiResponse(404);
            }
            var leadTransferDTOs = _mapper.Map<LeadTransferDTO>(lead);
            return new ApiOkResponse(leadTransferDTOs);
        }

        public async Task<ApiResponse> DropLead(HttpContext context, long id, DropLeadReceivingDTO dropLeadReceivingDTO)
        {

            var leadToUpdate = await _leadRepo.FindLeadById(id);
            if (leadToUpdate == null)
            {
                return new ApiResponse(404);
            }
            
            leadToUpdate.DropReasonId = dropLeadReceivingDTO.DropReasonId;
            leadToUpdate.DropLearning = dropLeadReceivingDTO.DropLearning;
            leadToUpdate.IsLeadDropped = true;
            var updatedLead = await _leadRepo.UpdateLead(leadToUpdate);

            if(updatedLead == null)
            {
                return new ApiResponse(500);
            }


            ModificationHistory history = new ModificationHistory()
            {
                ModelChanged = "Lead",
                ChangeSummary = $"this lead was dropped by user with id: {context.GetLoggedInUserId()}",
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedLead.Id
            };

            await _historyRepo.SaveHistory(history);

            var leadTransferDTOs = _mapper.Map<LeadTransferDTO>(updatedLead);
            return new ApiOkResponse(leadTransferDTOs);
        }
        public async Task<ApiResponse> UpdateLead(HttpContext context, long id, LeadReceivingDTO leadReceivingDTO)
        {

            var leadToUpdate = await _leadRepo.FindLeadById(id);
            if (leadToUpdate == null)
            {
                return new ApiResponse(404);
            }

            var summary = $"Initial details before change, \n {leadToUpdate.ToString()} \n";

            leadToUpdate.LeadTypeId = leadReceivingDTO.LeadTypeId;
            leadToUpdate.LeadOriginId = leadReceivingDTO.LeadOriginId;
            leadToUpdate.Industry = leadReceivingDTO.Industry;
            leadToUpdate.RCNumber = leadReceivingDTO.RCNumber;
            leadToUpdate.GroupName = leadReceivingDTO.GroupName;
            leadToUpdate.GroupTypeId = leadReceivingDTO.GroupTypeId;
            leadToUpdate.LogoUrl = leadReceivingDTO.LogoUrl;

            var updatedLead = await _leadRepo.UpdateLead(leadToUpdate);

            if(updatedLead == null)
            {
                return new ApiResponse(500);
            }

            summary += $"Details after change, \n {updatedLead.ToString()} \n";

            ModificationHistory history = new ModificationHistory()
            {
                ModelChanged = "Lead",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedLead.Id
            };

            await _historyRepo.SaveHistory(history);

            var leadTransferDTOs = _mapper.Map<LeadTransferDTO>(updatedLead);
            return new ApiOkResponse(leadTransferDTOs);


        }

        public async Task<ApiResponse> UpdateLeadStagesStatus(long leadId, LeadStages stage, LeadCaptureReceivingDTO leadCaptureReceivingDTO = null)
        {
            var leadToUpdate = await _leadRepo.FindLeadById(leadId);
            if (leadToUpdate == null)
            {
                return new ApiResponse(404);
            }
            switch (stage)
            {
                case LeadStages.Capture:
                    leadToUpdate.LeadCaptureStatus = true;
                    leadToUpdate.LeadCaptureDocumentUrl = leadCaptureReceivingDTO.LeadCaptureDocumentUrl;
                    break;
                case LeadStages.Qualification:
                    leadToUpdate.LeadQualificationStatus = true;
                    leadToUpdate.TimeMovedToLeadQualification = DateTime.Now;
                    break;
                case LeadStages.Opportunity:
                    leadToUpdate.LeadOpportunityStatus = true;
                    leadToUpdate.TimeMovedToLeadOpportunity = DateTime.Now;
                    break;
                case LeadStages.Closure:
                    leadToUpdate.TimeMovedToLeadClosure = DateTime.Now;
                    leadToUpdate.LeadClosureStatus = true;
                    break;
                case LeadStages.Conversion:
                    leadToUpdate.TimeConvertedToClient = DateTime.Now;
                    leadToUpdate.LeadConversionStatus = true;
                    break;
            }

            var updatedLead = await _leadRepo.UpdateLead(leadToUpdate);

            if(updatedLead == null)
            {
                return new ApiResponse(500);
            }

            var leadTransferDTOs = _mapper.Map<LeadTransferDTO>(updatedLead);
            return new ApiOkResponse(leadTransferDTOs);
        }


        public async Task<ApiResponse> ConvertLeadToClient(HttpContext context,long leadId)
        {
            var isConverted = await _leadConversionService.ConvertLeadToClient(leadId, context.GetLoggedInUserId());
                    
                if(isConverted)
                {
                    return new ApiOkResponse(true);
                }
                else
                {
                    return new ApiResponse(500);
                }

        }

        public async Task<ApiResponse> ApproveQuoteService(HttpContext context, long leadId, long quoteServiceId, long sequence)
        {
            try
            {
                /*var lead = await _context.Leads.Where(x => x.Id == leadId)
                    .Include(x => x.LeadDivisions)
                    .ThenInclude(x => x.Quote).SingleOrDefaultAsync();

                if (lead == null)
                {
                    return new ApiResponse(500);
                }

                var quotes = new List<Quote>();

                foreach (var leadDivision in lead.LeadDivisions)
                {
                    quotes.Add(leadDivision.Quote);
                }*/

                var quoteService = _context.QuoteServices.Where(x => x.Id == quoteServiceId)
                    .Include(x => x.Quote).SingleOrDefault();

                if(quoteService == null)
                {
                    return new ApiResponse(500);
                }

                var approvals = await _context.Approvals.Where(x => x.QuoteServiceId == quoteServiceId).ToListAsync();

                var approval = approvals.SingleOrDefault(x => x.Sequence == sequence);

                if(approval == null)
                {
                    return new ApiResponse(500);
                }

                approval.IsApproved = true;
                approval.DateTimeApproved = DateTime.Now;

                _context.Approvals.Update(approval);
                await _context.SaveChangesAsync();

                var history = new ModificationHistory()
                {
                    ModelChanged = "Approval",
                    ChangeSummary = $"Approval with QuoteServiceId: {quoteServiceId} was approved by user with userid {context.GetLoggedInUserId()}",
                    ChangedById = context.GetLoggedInUserId(),
                    ModifiedModelId = approval.Id
                };

                await _modificationRepo.SaveHistory(history);

                var otherApprovalApproved = approvals.Where(x => x.Sequence != sequence).All(x => x.IsApproved);

                // First exit scenario.
                // Only Approval is updated.
                // Other Quote Service Approvals not approved.
                if (!otherApprovalApproved)
                {
                    return new ApiOkResponse(true);
                }

                var quote = quoteService.Quote;

                if (quote == null)
                {
                    return new ApiResponse(500);
                }
                
                quote.IsApproved = true;
                _context.Quotes.Update(quote);
                await _context.SaveChangesAsync();
            
                return new ApiOkResponse(true);
                
                /*var allQuoteServicesApprovalsApproved = true;
                foreach (var qs in quote.QuoteServices)
                {
                    var theApprovals = await _context.Approvals.Where(x => x.QuoteServiceId == qs.Id).ToListAsync();

                    var quoteServiceApproved = theApprovals.All(x => x.IsApproved);
                    if (!quoteServiceApproved) 
                    {
                        allQuoteServicesApprovalsApproved = false;
                        break;
                    }
                }

                // Second exit scenario.
                // All Approvals for Quote Service Approved.
                // All Approvals for All Quote Services not approved.
                if (!allQuoteServicesApprovalsApproved)
                {
                    return new ApiOkResponse(true);
                }*/
                
                /*var otherQuotesApproved = quotes.Where(x => x.Id != quote.Id).All(x => x.IsApproved);

                // Second exit scenario.
                // All Quote's Quote Service(s) Approved.
                // All Quotes not approved.
                if (!otherQuotesApproved)
                {
                    return new ApiOkResponse(true);
                }

                bool converted = await _leadConversionService.ConvertLeadToClient(leadId, context.GetLoggedInUserId());
                if (converted)
                {
                    return new ApiOkResponse(true);
                }
                else
                {
                    return new ApiResponse(500);
                }*/
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                _logger.LogError(e.StackTrace);
                return new ApiResponse(500, e.Message);
            }
        }

        public async Task<ApiResponse> DisapproveQuoteService(HttpContext context, long leadId, long quoteServiceId, long sequence)
        {
            try
            {
                var lead = await _context.Leads.FindAsync(leadId);

                if (lead == null)
                {
                    return new ApiResponse(500);
                }

                lead.IsLeadDropped = true;
                _context.Leads.Update(lead);
                await _context.SaveChangesAsync();

                return new ApiOkResponse(true);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                _logger.LogError(e.StackTrace);
                return new ApiResponse(500, e.Message);
            }
        }
    }
}

