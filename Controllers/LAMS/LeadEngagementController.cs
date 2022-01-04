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
        public async Task<ApiCommonResponse> GetLeadEngagement()
        {
            return await _leadEngagementService.GetAllLeadEngagement();
        }
        [HttpGet("caption/{name}")]
        public async Task<ApiCommonResponse> GetByCaption(string name)
        {
            return await _leadEngagementService.GetLeadEngagementByName(name);
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _leadEngagementService.GetLeadEngagementById(id);
        }

        [HttpGet("GetLeadEngagementsByLeadId/{leadId}")]
        public async Task<ApiCommonResponse> FindLeadEngagementsByLeadId(long leadId)
        {
            return await _leadEngagementService.FindLeadEngagementsByLeadId(leadId);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewLeadEngagement(LeadEngagementReceivingDTO leadEngagementReceiving)
        {
            return await _leadEngagementService.AddLeadEngagement(HttpContext, leadEngagementReceiving);
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, LeadEngagementReceivingDTO leadEngagementReceivingDTO)
        {
            return await _leadEngagementService.UpdateLeadEngagement(HttpContext, id, leadEngagementReceivingDTO);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _leadEngagementService.DeleteLeadEngagement(id);
        }
    }
}