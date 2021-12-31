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
        public async Task<ApiCommonResponse> GetApprovalLimit()
        {
            return await _approvalLimitService.GetAllApprovalLimit();
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewApprovalLimit(ApprovalLimitReceivingDTO approvalLimitReceiving)
        {
            return await _approvalLimitService.AddApprovalLimit(HttpContext, approvalLimitReceiving);
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, ApprovalLimitReceivingDTO approvalLimitReceiving)
        {
            return await _approvalLimitService.UpdateApprovalLimit(HttpContext, id, approvalLimitReceiving);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _approvalLimitService.DeleteApprovalLimit(id);
        }

    }
}