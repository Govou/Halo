using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HaloBiz.Data;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs.LAMS;
using HaloBiz.DTOs.TransferDTOs.LAMS;
using HaloBiz.Helpers;
using HaloBiz.Model;
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
        private readonly DataContext _context;
        private readonly IReferenceNumberRepository _refNumberRepo;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly ILeadRepository _leadRepo;
        private readonly IMapper _mapper;

        public LeadServiceImpl(
                                DataContext context,
                                IReferenceNumberRepository refNumberRepo,
                                IModificationHistoryRepository historyRepo,
                                ILeadRepository leadRepo,
                                ILogger<LeadServiceImpl> logger,
                                IMapper mapper
                                )
        {
            this._mapper = mapper;
            this._context = context;
            this._refNumberRepo = refNumberRepo;
            this._historyRepo = historyRepo;
            this._leadRepo = leadRepo;
            this._logger = logger;
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

        public async Task<ApiResponse> UpdateLeadStagesStatus(long leadId, LeadStages stage)
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


        public async Task<ApiResponse> ConvertLeadToClient(long leadId)
        {
            using(var transaction =  await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var lead = await GetLeadById(leadId);
                    

                    return new ApiOkResponse(true);
                }
                catch (System.Exception e)
                {
                    _logger.LogError(e.Message);
                    _logger.LogError(e.StackTrace);
                    return new ApiResponse(500);
                }

            }

        
        }

        // private async Task<Quote> ConvertQuoteToContract(long leadDivisionId, DataContext context)
        // {
        //     var quotes = await context.LeadDivisions
        //         .Include(x => x.Quotes)
        //         .
        //         .FirstOrDefaultAsync(x => x.Id == leadId);
                
        // }
    }
}

