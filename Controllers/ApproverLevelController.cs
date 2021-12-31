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
    public class ApproverLevelController : ControllerBase
    {
        private readonly IApproverLevelService _approverLevelService;

        public ApproverLevelController(IApproverLevelService approverLevelService)
        {
            this._approverLevelService = approverLevelService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetApproverLevel()
        {
            var response = await _approverLevelService.GetAllApproverLevel();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var approverLevel = ((ApiOkResponse)response).Result;
            return Ok(approverLevel);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewApproverLevel(ApproverLevelReceivingDTO approverLevelReceiving)
        {
            var response = await _approverLevelService.AddApproverLevel(HttpContext, approverLevelReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var approverLevel = ((ApiOkResponse)response).Result;
            return Ok(approverLevel);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, ApproverLevelReceivingDTO approverLevelReceiving)
        {
            var response = await _approverLevelService.UpdateApproverLevel(HttpContext, id, approverLevelReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var approverLevel = ((ApiOkResponse)response).Result;
            return Ok(approverLevel);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            var response = await _approverLevelService.DeleteApproverLevel(id);
            return StatusCode(response.StatusCode);
        }

    }
}