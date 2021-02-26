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

namespace HaloBiz.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ApprovalController : ControllerBase
    {
        private readonly IApprovalService _approvalService;

        public ApprovalController(IApprovalService approvalService)
        {
            this._approvalService = approvalService;
        }

        [HttpGet("")]
        public async Task<ActionResult> GetApproval()
        {
            var response = await _approvalService.GetAllApproval();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var approval = ((ApiOkResponse)response).Result;
            return Ok(approval);
        }

        [HttpGet("GetPendingApprovals")]
        public async Task<ActionResult> GetPendingApprovals()
        {
            var response = await _approvalService.GetPendingApprovals();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var approval = ((ApiOkResponse)response).Result;
            return Ok(approval);
        }

        [HttpGet("GetPendingApprovalsByServiceId/{serviceId}")]
        public async Task<ActionResult> GetPendingApprovalsByServiceId(long serviceId)
        {
            var response = await _approvalService.GetPendingApprovalsByServiceId(serviceId);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var approval = ((ApiOkResponse)response).Result;
            return Ok(approval);
        }

        [HttpGet("GetPendingApprovalsByQuoteId/{quoteId}")]
        public async Task<ActionResult> GetPendingApprovalsByQuoteId(long quoteId)
        {
            var response = await _approvalService.GetPendingApprovalsByQuoteId(quoteId);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var approval = ((ApiOkResponse)response).Result;
            return Ok(approval);
        }

        [HttpGet("GetUserPendingApprovals")]
        public async Task<ActionResult> GetUserPendingApprovals()
        {
            var response = await _approvalService.GetUserPendingApprovals(HttpContext);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var approval = ((ApiOkResponse)response).Result;
            return Ok(approval);
        }

        [HttpPost("")]
        public async Task<ActionResult> AddNewApproval(ApprovalReceivingDTO approvalReceiving)
        {
            var response = await _approvalService.AddApproval(HttpContext, approvalReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var approval = ((ApiOkResponse)response).Result;
            return Ok(approval);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, ApprovalReceivingDTO approvalReceiving)
        {
            var response = await _approvalService.UpdateApproval(HttpContext, id, approvalReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var approval = ((ApiOkResponse)response).Result;
            return Ok(approval);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            var response = await _approvalService.DeleteApproval(id);
            return StatusCode(response.StatusCode);
        }

    }
}