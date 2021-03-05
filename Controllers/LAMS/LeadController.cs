using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
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
    public class LeadController : ControllerBase
    {
        private readonly ILeadService _leadService;

        public LeadController(ILeadService leadService)
        {
            this._leadService = leadService;
        }

        [HttpGet("")]
        public async Task<ActionResult> GetLead()
        {
            var response = await _leadService.GetAllLead();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var lead = ((ApiOkResponse)response).Result;
            return Ok(lead);
        }

        [HttpGet("GetUnApprovedLeads")]
        public async Task<ActionResult> GetUnApprovedLeads()
        {
            var response = await _leadService.GetAllUnApprovedLeads();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var lead = ((ApiOkResponse)response).Result;
            return Ok(lead);
        }

        [HttpGet("ReferenceNumber/{refNo}")]
        public async Task<ActionResult> GetByCaption(string refNo)
        {
            var response = await _leadService.GetLeadByReferenceNumber(refNo);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var lead = ((ApiOkResponse)response).Result;
            return Ok(lead);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var response = await _leadService.GetLeadById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var lead = ((ApiOkResponse)response).Result;
            return Ok(lead);
        }
        
        [HttpPut("SetUpLeadForApproval/{id}")]
        public async Task<ActionResult> SetUpLeadForApproval(long id)
        {
            var response = await _leadService.SetUpLeadForApproval(HttpContext, id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var lead = ((ApiOkResponse)response).Result;
            return Ok(lead);
        }

        [HttpPut("ConvertLeadToClient/{id}")]
        public async Task<ActionResult> ConvertLeadToClient(long id)
        {
            var response = await _leadService.ConvertLeadToClient(HttpContext, id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var lead = ((ApiOkResponse)response).Result;
            return Ok(lead);
        }

        [HttpPut("approve-quote-service/{leadId}/{quoteServiceId}/{sequence}")]
        public async Task<IActionResult> ApproveQuoteServiceById(long leadId, long quoteServiceId, long sequence)
        {
            var response = await _leadService.ApproveQuoteService(HttpContext, leadId, quoteServiceId, sequence);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var serviceGroup = ((ApiOkResponse)response).Result;
            return Ok(serviceGroup);
        }

        [HttpPost("")]
        public async Task<ActionResult> AddNewLead(LeadReceivingDTO leadReceiving)
        {
            var response = await _leadService.AddLead(HttpContext, leadReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var lead = ((ApiOkResponse)response).Result;
            return Ok(lead);
        }

        [HttpPut("{id}/UpdateLeadCaptured")]
        public async Task<IActionResult> UpdateCaptureStatus(long id, LeadCaptureReceivingDTO leadCaptureReceivingDTO)
        {
            var response = await _leadService.UpdateLeadStagesStatus(id, LeadStages.Capture, leadCaptureReceivingDTO);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var lead = ((ApiOkResponse)response).Result;
            return Ok(lead);
        }

        [HttpPut("{id}/UpdateLeadOpportunity")]
        public async Task<IActionResult> UpdateOpportunityStatus(long id)
        {
            var response = await _leadService.UpdateLeadStagesStatus(id, LeadStages.Opportunity);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var lead = ((ApiOkResponse)response).Result;
            return Ok(lead);
        }

        [HttpPut("{id}/UpdateLeadClosure")]
        public async Task<IActionResult> UpdateClosureStatus(long id)
        {
            var response = await _leadService.UpdateLeadStagesStatus(id, LeadStages.Closure);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var lead = ((ApiOkResponse)response).Result;
            return Ok(lead);
        }

        [HttpPut("{id}/DropLead")]
        public async Task<IActionResult> DropLead(long id, DropLeadReceivingDTO dropReasonDto)
        {
            var response = await _leadService.DropLead(HttpContext, id, dropReasonDto);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var lead = ((ApiOkResponse)response).Result;
            return Ok(lead);
        }

        // [HttpPut("{id}/UpdateLeadConversion")]
        // public async Task<IActionResult> UpdateConversionStatus(long id)
        // {
        //     var response = await _leadService.UpdateLeadStagesStatus(id, LeadStages.Conversion);
        //     if (response.StatusCode >= 400)
        //         return StatusCode(response.StatusCode, response);
        //     var lead = ((ApiOkResponse)response).Result;
        //     return Ok(lead);
        // }

        [HttpPut("{id}/UpdateLeadQualification")]
        public async Task<IActionResult> UpdateQualificationStatus(long id)
        {
            var response = await _leadService.UpdateLeadStagesStatus(id, LeadStages.Qualification);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var lead = ((ApiOkResponse)response).Result;
            return Ok(lead);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, LeadReceivingDTO leadReceivingDTO)
        {
            var response = await _leadService.UpdateLead(HttpContext, id, leadReceivingDTO);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var lead = ((ApiOkResponse)response).Result;
            return Ok(lead);
        }

    }
}