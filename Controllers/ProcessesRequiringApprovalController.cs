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
    public class ProcessesRequiringApprovalController : ControllerBase
    {
        private readonly IProcessesRequiringApprovalService _approverLevelService;

        public ProcessesRequiringApprovalController(IProcessesRequiringApprovalService approverLevelService)
        {
            this._approverLevelService = approverLevelService;
        }

        [HttpGet("")]
        public async Task<ActionResult> GetProcessesRequiringApproval()
        {
            var response = await _approverLevelService.GetAllProcessesRequiringApproval();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var approverLevel = ((ApiOkResponse)response).Result;
            return Ok(approverLevel);
        }

        [HttpPost("")]
        public async Task<ActionResult> AddNewProcessesRequiringApproval(ProcessesRequiringApprovalReceivingDTO approverLevelReceiving)
        {
            var response = await _approverLevelService.AddProcessesRequiringApproval(HttpContext, approverLevelReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var approverLevel = ((ApiOkResponse)response).Result;
            return Ok(approverLevel);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, ProcessesRequiringApprovalReceivingDTO approverLevelReceiving)
        {
            var response = await _approverLevelService.UpdateProcessesRequiringApproval(HttpContext, id, approverLevelReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var approverLevel = ((ApiOkResponse)response).Result;
            return Ok(approverLevel);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            var response = await _approverLevelService.DeleteProcessesRequiringApproval(id);
            return StatusCode(response.StatusCode);
        }

    }
}