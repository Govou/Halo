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
    public class OtherLeadCaptureInfoController : ControllerBase
    {
        private readonly IOtherLeadCaptureInfoService _otherLeadCaptureInfoService;

        public OtherLeadCaptureInfoController(IOtherLeadCaptureInfoService otherLeadCaptureInfoService)
        {
            this._otherLeadCaptureInfoService = otherLeadCaptureInfoService;
        }

        [HttpGet("")]
        public async Task<ActionResult> GetOtherLeadCaptureInfo()
        {
            var response = await _otherLeadCaptureInfoService.GetAllOtherLeadCaptureInfo();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var otherLeadCaptureInfo = ((ApiOkResponse)response).Result;
            return Ok(otherLeadCaptureInfo);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var response = await _otherLeadCaptureInfoService.GetOtherLeadCaptureInfoById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var otherLeadCaptureInfo = ((ApiOkResponse)response).Result;
            return Ok(otherLeadCaptureInfo);
        }
        [HttpGet("LeadDivision/{leadDivisionId}")]
        public async Task<ActionResult> GetByLeadDivisionId(long leadDivisionId)
        {
            var response = await _otherLeadCaptureInfoService.GetOtherLeadCaptureInfoByLeadDivisionId(leadDivisionId);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var otherLeadCaptureInfo = ((ApiOkResponse)response).Result;
            return Ok(otherLeadCaptureInfo);
        }

        [HttpPost("")]
        public async Task<ActionResult> AddNewOtherLeadCaptureInfo(OtherLeadCaptureInfoReceivingDTO otherLeadCaptureInfoReceiving)
        {
            var response = await _otherLeadCaptureInfoService.AddOtherLeadCaptureInfo(HttpContext, otherLeadCaptureInfoReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var otherLeadCaptureInfo = ((ApiOkResponse)response).Result;
            return Ok(otherLeadCaptureInfo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, OtherLeadCaptureInfoReceivingDTO otherLeadCaptureInfoReceivingDTO)
        {
            var response = await _otherLeadCaptureInfoService.UpdateOtherLeadCaptureInfo(HttpContext, id, otherLeadCaptureInfoReceivingDTO);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var otherLeadCaptureInfo = ((ApiOkResponse)response).Result;
            return Ok(otherLeadCaptureInfo);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            var response = await _otherLeadCaptureInfoService.DeleteOtherLeadCaptureInfo(id);
            return StatusCode(response.StatusCode);
        }
    }
}