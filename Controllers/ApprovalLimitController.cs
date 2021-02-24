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
    public class ApprovalLimitController : ControllerBase
    {
        private readonly IApprovalLimitService _approvalLimitService;

        public ApprovalLimitController(IApprovalLimitService approvalLimitService)
        {
            this._approvalLimitService = approvalLimitService;
        }

        [HttpGet("")]
        public async Task<ActionResult> GetApprovalLimit()
        {
            var response = await _approvalLimitService.GetAllApprovalLimit();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var approvalLimit = ((ApiOkResponse)response).Result;
            return Ok(approvalLimit);
        }

        [HttpPost("")]
        public async Task<ActionResult> AddNewApprovalLimit(ApprovalLimitReceivingDTO approvalLimitReceiving)
        {
            var response = await _approvalLimitService.AddApprovalLimit(HttpContext, approvalLimitReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var approvalLimit = ((ApiOkResponse)response).Result;
            return Ok(approvalLimit);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, ApprovalLimitReceivingDTO approvalLimitReceiving)
        {
            var response = await _approvalLimitService.UpdateApprovalLimit(HttpContext, id, approvalLimitReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var approvalLimit = ((ApiOkResponse)response).Result;
            return Ok(approvalLimit);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            var response = await _approvalLimitService.DeleteApprovalLimit(id);
            return StatusCode(response.StatusCode);
        }

    }
}