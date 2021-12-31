using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs.LAMS;
using HaloBiz.MyServices;
using HaloBiz.MyServices.LAMS;
using Microsoft.AspNetCore.Mvc;
//using Controllers.Models;

namespace HaloBiz.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EngagementReasonController : ControllerBase
    {
        private readonly IEngagementReasonService _engagementReasonService;

        public EngagementReasonController(IEngagementReasonService engagementReasonService)
        {
            this._engagementReasonService = engagementReasonService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetEngagementReason()
        {
            var response = await _engagementReasonService.GetAllEngagementReason();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var engagementReason = ((ApiOkResponse)response).Result;
            return Ok(engagementReason);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewEngagementReason(EngagementReasonReceivingDTO engagementReasonReceiving)
        {
            var response = await _engagementReasonService.AddEngagementReason(HttpContext, engagementReasonReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var engagementReason = ((ApiOkResponse)response).Result;
            return Ok(engagementReason);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, EngagementReasonReceivingDTO engagementReasonReceiving)
        {
            var response = await _engagementReasonService.UpdateEngagementReason(HttpContext, id, engagementReasonReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var engagementReason = ((ApiOkResponse)response).Result;
            return Ok(engagementReason);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            var response = await _engagementReasonService.DeleteEngagementReason(id);
            return StatusCode(response.StatusCode);
        }

    }
}