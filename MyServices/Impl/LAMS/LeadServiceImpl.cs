using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HalobizMigrations.Data;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs.LAMS;
using HaloBiz.DTOs.TransferDTOs.LAMS;
using HaloBiz.Helpers;
using HalobizMigrations.Models;
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
        private readonly HalobizContext _context;

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
                                HalobizContext context
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

        public async Task<ApiCommonResponse> AddLead(HttpContext context, LeadReceivingDTO leadReceivingDTO)
        {
            var lead = _mapper.Map<Lead>(leadReceivingDTO);
            lead.CreatedById = context.GetLoggedInUserId();
            var referenceNumber = await _refNumberRepo.GetReferenceNumber();
            if(referenceNumber == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            lead.ReferenceNo = referenceNumber.ReferenceNo.GenerateReferenceNumber();
            referenceNumber.ReferenceNo = referenceNumber.ReferenceNo + 1;
            var isUpdatedRefNumber = await _refNumberRepo.UpdateReferenceNumber(referenceNumber);

            if(!isUpdatedRefNumber)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            var savedLead = await _leadRepo.SaveLead(lead);
            if (savedLead == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var leadTransferDTO = _mapper.Map<LeadTransferDTO>(savedLead);
            return CommonResponse.Send(ResponseCodes.SUCCESS,leadTransferDTO);
        }

        public async Task<ApiCommonResponse> GetAllLead()
        {
            var leads = await _leadRepo.FindAllLead();
            if (leads == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var leadTransferDTO = _mapper.Map<IEnumerable<LeadTransferDTO>>(leads);
            return CommonResponse.Send(ResponseCodes.SUCCESS,leadTransferDTO);
        }

        public async Task<ApiCommonResponse> GetUserLeads(HttpContext context)
        {
            var leads = await _leadRepo.FindUserLeads(context.GetLoggedInUserId());
            if (leads == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var leadTransferDTO = _mapper.Map<IEnumerable<LeadTransferDTO>>(leads);
            return CommonResponse.Send(ResponseCodes.SUCCESS,leadTransferDTO);
        }

        public async Task<ApiCommonResponse> GetAllUnApprovedLeads()
        {
            var leads = await _leadRepo.FindAllUnApprovedLeads();
            if (leads == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var leadTransferDTO = _mapper.Map<IEnumerable<LeadTransferDTO>>(leads);
            return CommonResponse.Send(ResponseCodes.SUCCESS,leadTransferDTO);
        }

        public async Task<ApiCommonResponse> SetUpLeadForApproval(HttpContext httpContext, long id)
        {
            var approvalsCreatedSuccessfully = await _approvalService.SetUpApprovalsForClientCreation(id, httpContext);
                    
            if(approvalsCreatedSuccessfully)
            {
                return CommonResponse.Send(ResponseCodes.SUCCESS);
            }
            else
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
        }

        public async Task<ApiCommonResponse> GetLeadById(long id)
        {
            var lead = await _leadRepo.FindLeadById(id);
            if (lead == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var leadTransferDTOs = _mapper.Map<LeadTransferDTO>(lead);
            return CommonResponse.Send(ResponseCodes.SUCCESS,leadTransferDTOs);
        }

        public async Task<ApiCommonResponse> GetLeadByReferenceNumber(string refNumber)
        {
            var lead = await _leadRepo.FindLeadByReferenceNo(refNumber);
            if (lead == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var leadTransferDTOs = _mapper.Map<LeadTransferDTO>(lead);
            return CommonResponse.Send(ResponseCodes.SUCCESS,leadTransferDTOs);
        }

        public async Task<ApiCommonResponse> DropLead(HttpContext context, long id, DropLeadReceivingDTO dropLeadReceivingDTO)
        {

            var leadToUpdate = await _leadRepo.FindLeadById(id);
            if (leadToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            
            leadToUpdate.DropReasonId = dropLeadReceivingDTO.DropReasonId;
            leadToUpdate.DropLearning = dropLeadReceivingDTO.DropLearning;
            leadToUpdate.IsLeadDropped = true;
            var updatedLead = await _leadRepo.UpdateLead(leadToUpdate);

            if(updatedLead == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
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
            return CommonResponse.Send(ResponseCodes.SUCCESS,leadTransferDTOs);
        }
        public async Task<ApiCommonResponse> UpdateLead(HttpContext context, long id, LeadReceivingDTO leadReceivingDTO)
        {

            var leadToUpdate = await _leadRepo.FindLeadById(id);
            if (leadToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            var summary = $"Initial details before change, \n {leadToUpdate.ToString()} \n";

            leadToUpdate.LeadTypeId = leadReceivingDTO.LeadTypeId;
            leadToUpdate.LeadOriginId = leadReceivingDTO.LeadOriginId;
            leadToUpdate.Industry = leadReceivingDTO.Industry;
            leadToUpdate.Rcnumber = leadReceivingDTO.RCNumber;
            leadToUpdate.GroupName = leadReceivingDTO.GroupName;
            leadToUpdate.GroupTypeId = leadReceivingDTO.GroupTypeId;
            leadToUpdate.LogoUrl = leadReceivingDTO.LogoUrl;

            var updatedLead = await _leadRepo.UpdateLead(leadToUpdate);

            if(updatedLead == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
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
            return CommonResponse.Send(ResponseCodes.SUCCESS,leadTransferDTOs);


        }

        public async Task<ApiCommonResponse> UpdateLeadStagesStatus(long leadId, LeadStages stage, LeadCaptureReceivingDTO leadCaptureReceivingDTO = null)
        {
            var leadToUpdate = await _context.Leads.FirstOrDefaultAsync(x => x.Id == leadId);
            
            if (leadToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
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
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            var leadTransferDTOs = _mapper.Map<LeadTransferDTO>(updatedLead);
            return CommonResponse.Send(ResponseCodes.SUCCESS,leadTransferDTOs);
        }


        public async Task<ApiCommonResponse> ConvertLeadToClient(HttpContext context,long leadId)
        {
            var (isConverted, message) = await _leadConversionService.ConvertLeadToClient(leadId, context.GetLoggedInUserId());
                    
                if(isConverted)
                {
                    return CommonResponse.Send(ResponseCodes.SUCCESS);
                }
                else
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE,null, message);
                }

        }

        public async Task<ApiCommonResponse> ApproveQuoteService(HttpContext context, long leadId, long quoteServiceId, long sequence)
        {
            try
            {

                var quoteService = _context.QuoteServices.Where(x => x.Id == quoteServiceId)
                    .Include(x => x.Quote).SingleOrDefault();

                if(quoteService == null)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
                }

                var approvals = await _context.Approvals.Where(x => x.QuoteServiceId == quoteServiceId).ToListAsync();

                var approval = approvals.SingleOrDefault(x => x.Sequence == sequence);

                if(approval == null)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
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
                    return CommonResponse.Send(ResponseCodes.SUCCESS);
                }

                var quote = quoteService.Quote;

                if (quote == null)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
                }
                
                quote.IsApproved = true;
                _context.Quotes.Update(quote);
                await _context.SaveChangesAsync();
            
                return CommonResponse.Send(ResponseCodes.SUCCESS);             
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                _logger.LogError(e.StackTrace);
                return CommonResponse.Send(ResponseCodes.FAILURE,null, e.Message);
            }
        }

        public async Task<ApiCommonResponse> DisapproveQuoteService(HttpContext context, long leadId, long quoteServiceId, long sequence)
        {
            try
            {
                var lead = await _context.Leads.FindAsync(leadId);

                if (lead == null)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
                }

                lead.IsLeadDropped = true;
                _context.Leads.Update(lead);
                await _context.SaveChangesAsync();

                return CommonResponse.Send(ResponseCodes.SUCCESS);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                _logger.LogError(e.StackTrace);
                return CommonResponse.Send(ResponseCodes.FAILURE,null, e.Message);
            }
        }
    }
}

