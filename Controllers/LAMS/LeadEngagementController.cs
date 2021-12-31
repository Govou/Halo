using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs.LAMS;
using HaloBiz.MyServices.LAMS;
using Microsoft.AspNetCore.Mvc;
//using Controllers.Models;

namespace Controllers.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LeadEngagementController : ControllerBase
    {
        private readonly ILeadEngagementService _leadEngagementService;

        public LeadEngagementController(ILeadEngagementService leadEngagementService)
        {
            this._leadEngagementService = leadEngagementService;
        }

        [HttpGet("")]
        public async Task<ActionResult> GetLeadEngagement()
        {
            var response = await _leadEngagementService.GetAllLeadEngagement();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var leadEngagement = ((ApiOkResponse)response).Result;
            return Ok(leadEngagement);
        }
        [HttpGet("caption/{name}")]
        public async Task<ActionResult> GetByCaption(string name)
        {
            var response = await _leadEngagementService.GetLeadEngagementByName(name);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var leadEngagement = ((ApiOkResponse)response).Result;
            return Ok(leadEngagement);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var response = await _leadEngagementService.GetLeadEngagementById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var leadEngagement = ((ApiOkResponse)response).Result;
            return Ok(leadEngagement);
        }

        [HttpGet("GetLeadEngagementsByLeadId/{leadId}")]
        public async Task<ActionResult> FindLeadEngagementsByLeadId(long leadId)
        {
            var response = await _leadEngagementService.FindLeadEngagementsByLeadId(leadId);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var leadEngagement = ((ApiOkResponse)response).Result;
            return Ok(leadEngagement);
        }

        [HttpPost("")]
        public async Task<ActionResult> AddNewLeadEngagement(LeadEngagementReceivingDTO leadEngagementReceiving)
        {
            var response = await _leadEngagementService.AddLeadEngagement(HttpContext, leadEngagementReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var leadEngagement = ((ApiOkResponse)response).Result;
            return Ok(leadEngagement);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, LeadEngagementReceivingDTO leadEngagementReceivingDTO)
        {
            var response = await _leadEngagementService.UpdateLeadEngagement(HttpContext, id, leadEngagementReceivingDTO);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var leadEngagement = ((ApiOkResponse)response).Result;
            return Ok(leadEngagement);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            var response = await _leadEngagementService.DeleteLeadEngagement(id);
            return StatusCode(response.StatusCode);
        }
    }
}