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
        public async Task<ApiCommonResponse> GetOtherLeadCaptureInfo()
        {
            return await _otherLeadCaptureInfoService.GetAllOtherLeadCaptureInfo();
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _otherLeadCaptureInfoService.GetOtherLeadCaptureInfoById(id);
        }

        [HttpGet("LeadDivision/{leadDivisionId}")]
        public async Task<ApiCommonResponse> GetByLeadDivisionId(long leadDivisionId)
        {
            return await _otherLeadCaptureInfoService.GetOtherLeadCaptureInfoByLeadDivisionId(leadDivisionId);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewOtherLeadCaptureInfo(OtherLeadCaptureInfoReceivingDTO otherLeadCaptureInfoReceiving)
        {
            return await _otherLeadCaptureInfoService.AddOtherLeadCaptureInfo(HttpContext, otherLeadCaptureInfoReceiving);
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, OtherLeadCaptureInfoReceivingDTO otherLeadCaptureInfoReceivingDTO)
        {
            return await _otherLeadCaptureInfoService.UpdateOtherLeadCaptureInfo(HttpContext, id, otherLeadCaptureInfoReceivingDTO);
   
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _otherLeadCaptureInfoService.DeleteOtherLeadCaptureInfo(id);
        }
    }
}