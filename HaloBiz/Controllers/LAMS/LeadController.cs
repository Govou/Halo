using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Halobiz.Common.Auths;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.ReceivingDTOs.LAMS;
using HaloBiz.DTOs.TransferDTOs.LAMS;
using HaloBiz.Helpers;
using HaloBiz.MyServices.LAMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HaloBiz.Controllers.LAMS
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ModuleName(HalobizModules.LeadAdministration,20)]

    public class LeadController : ControllerBase
    {
        private readonly ILeadService _leadService;

        public LeadController(ILeadService leadService)
        {
            this._leadService = leadService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetLead()
        {
            return await _leadService.GetAllLead();
        }

        [HttpGet("GetUserLeads")]
        public async Task<ApiCommonResponse> GetUserLeads()
        {
            return await _leadService.GetUserLeads(HttpContext); 
        }

        [HttpGet("GetUnApprovedLeads")]
        public async Task<ApiCommonResponse> GetUnApprovedLeads()
        {
            return await _leadService.GetAllUnApprovedLeads();
        }

        [HttpGet("ReferenceNumber/{refNo}")]
        public async Task<ApiCommonResponse> GetByCaption(string refNo)
        {
            return await _leadService.GetLeadByReferenceNumber(refNo);
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _leadService.GetLeadById(id);
        }
        
        [HttpPut("SetUpLeadForApproval/{id}")]
        public async Task<ApiCommonResponse> SetUpLeadForApproval(long id)
        {
            return await _leadService.SetUpLeadForApproval(HttpContext, id);
        }

        [HttpPut("ConvertLeadToClient/{id}")]
        public async Task<ApiCommonResponse> ConvertLeadToClient(long id)
        {
            return await _leadService.ConvertLeadToClient(HttpContext, id);
        }

        [HttpPut("approve-quote-service/{leadId}/{quoteServiceId}/{sequence}")]
        public async Task<ApiCommonResponse> ApproveQuoteServiceById(long leadId, long quoteServiceId, long sequence)
        {
            return await _leadService.ApproveQuoteService(HttpContext, leadId, quoteServiceId, sequence);
        }

        [HttpPut("disapprove-quote-service/{leadId}/{quoteServiceId}/{sequence}")]
        public async Task<ApiCommonResponse> DisapproveQuoteServiceById(long leadId, long quoteServiceId, long sequence)
        {
            return await _leadService.DisapproveQuoteService(HttpContext, leadId, quoteServiceId, sequence);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewLead(LeadReceivingDTO leadReceiving)
        {
            return await _leadService.AddLead(HttpContext, leadReceiving);
        }

        [HttpPut("{id}/UpdateLeadCaptured")]
        public async Task<ApiCommonResponse> UpdateCaptureStatus(long id, LeadCaptureReceivingDTO leadCaptureReceivingDTO)
        {
            return await _leadService.UpdateLeadStagesStatus(id, LeadStages.Capture, leadCaptureReceivingDTO);
        }

        [HttpPut("{id}/UpdateLeadOpportunity")]
        public async Task<ApiCommonResponse> UpdateOpportunityStatus(long id)
        {
            return await _leadService.UpdateLeadStagesStatus(id, LeadStages.Opportunity);
        }

        [HttpPut("{id}/UpdateLeadClosure")]
        public async Task<ApiCommonResponse> UpdateClosureStatus(long id)
        {
            return await _leadService.UpdateLeadStagesStatus(id, LeadStages.Closure);
        }

        [HttpPut("{id}/DropLead")]
        public async Task<ApiCommonResponse> DropLead(long id, DropLeadReceivingDTO dropReasonDto)
        {
            return await _leadService.DropLead(HttpContext, id, dropReasonDto);
        }

        // [HttpPut("{id}/UpdateLeadConversion")]
        // public async Task<ApiCommonResponse> UpdateConversionStatus(long id)
        // {
        //     return await _leadService.UpdateLeadStagesStatus(id, LeadStages.Conversion);
        //     
        //         
        //     var lead = ((ApiOkResponse)response).Result;
        //     return Ok(lead);
        // }

        [HttpPut("{id}/UpdateLeadQualification")]
        public async Task<ApiCommonResponse> UpdateQualificationStatus(long id)
        {
            return await _leadService.UpdateLeadStagesStatus(id, LeadStages.Qualification);
        }


        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, LeadReceivingDTO leadReceivingDTO)
        {
            return await _leadService.UpdateLead(HttpContext, id, leadReceivingDTO);
        }

    }
}