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
    public class SuspectQualificationController : ControllerBase
    {
        private readonly ISuspectQualificationService _SuspectQualificationService;

        public SuspectQualificationController(ISuspectQualificationService suspectQualificationService)
        {
            this._SuspectQualificationService = suspectQualificationService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetSuspectQualification()
        {
            var response = await _SuspectQualificationService.GetAllSuspectQualification();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var SuspectQualification = ((ApiOkResponse)response).Result;
            return Ok(SuspectQualification);
        }

        [HttpGet("GetUserSuspectQualification")]
        public async Task<ApiCommonResponse> GetUserSuspectQualification()
        {
            var response = await _SuspectQualificationService.GetUserSuspectQualification(HttpContext);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var SuspectQualification = ((ApiOkResponse)response).Result;
            return Ok(SuspectQualification);
        }

        /*[HttpGet("caption/{name}")]
        public async Task<ApiCommonResponse> GetByCaption(string name)
        {
            var response = await _SuspectQualificationService.GetSuspectQualificationByName(name);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var SuspectQualification = ((ApiOkResponse)response).Result;
            return Ok(SuspectQualification);
        }*/

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            var response = await _SuspectQualificationService.GetSuspectQualificationById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var SuspectQualification = ((ApiOkResponse)response).Result;
            return Ok(SuspectQualification);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewSuspectQualification(SuspectQualificationReceivingDTO SuspectQualificationReceiving)
        {
            var response = await _SuspectQualificationService.AddSuspectQualification(HttpContext, SuspectQualificationReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var SuspectQualification = ((ApiOkResponse)response).Result;
            return Ok(SuspectQualification);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, SuspectQualificationReceivingDTO SuspectQualificationReceiving)
        {
            var response = await _SuspectQualificationService.UpdateSuspectQualification(HttpContext, id, SuspectQualificationReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var SuspectQualification = ((ApiOkResponse)response).Result;
            return Ok(SuspectQualification);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            var response = await _SuspectQualificationService.DeleteSuspectQualification(id);
            return StatusCode(response.StatusCode);
        }
    }
}
