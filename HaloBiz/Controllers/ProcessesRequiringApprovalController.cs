using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Halobiz.Common.DTOs.ApiDTOs;
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
    public class ProcessesRequiringApprovalController : ControllerBase
    {
        private readonly IProcessesRequiringApprovalService _approverLevelService;

        public ProcessesRequiringApprovalController(IProcessesRequiringApprovalService approverLevelService)
        {
            this._approverLevelService = approverLevelService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetProcessesRequiringApproval()
        {
            return await _approverLevelService.GetAllProcessesRequiringApproval();
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewProcessesRequiringApproval(ProcessesRequiringApprovalReceivingDTO approverLevelReceiving)
        {
            return await _approverLevelService.AddProcessesRequiringApproval(HttpContext, approverLevelReceiving);
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, ProcessesRequiringApprovalReceivingDTO approverLevelReceiving)
        {
            return await _approverLevelService.UpdateProcessesRequiringApproval(HttpContext, id, approverLevelReceiving);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _approverLevelService.DeleteProcessesRequiringApproval(id);
        }

    }
}