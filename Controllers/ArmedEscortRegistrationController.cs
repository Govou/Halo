using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.MyServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ArmedEscortRegistrationController : ControllerBase
    {
        private readonly IArmedEscortRegistrationService _armedEscortService;

        public ArmedEscortRegistrationController(IArmedEscortRegistrationService armedEscortService)
        {
            _armedEscortService = armedEscortService;
        }

        [HttpGet("GetAllArmedEscortProfiless")]
        public async Task<ApiCommonResponse> GetAllArmedEscortProfiless()
        {
            var response = await _armedEscortService.GetAllArmedEscorts();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var cRank = ((ApiOkResponse)response).Result;
            return Ok(cRank);
        }

        [HttpGet("GetAllArmedEscortTies")]
        public async Task<ApiCommonResponse> GetAllArmedEscortTies()
        {
            var response = await _armedEscortService.GetAllArmedEscortTies();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var cRank = ((ApiOkResponse)response).Result;
            return Ok(cRank);
        }
        [HttpGet("GetProfileById/{id}")]
        public async Task<ApiCommonResponse> GetProfileById(long id)
        {
            var response = await _armedEscortService.GetArmedEscortById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Rank = ((ApiOkResponse)response).Result;
            return Ok(Rank);
        }

        [HttpGet("GetProfileTieById/{id}")]
        public async Task<ApiCommonResponse> GetProfileTieById(long id)
        {
            var response = await _armedEscortService.GetArmedEscortTieById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Rank = ((ApiOkResponse)response).Result;
            return Ok(Rank);
        }

        [HttpPost("AddNewProfile")]
        public async Task<ApiCommonResponse> AddNewProfile(ArmedEscortProfileReceivingDTO ReceivingDTO)
        {
            var response = await _armedEscortService.AddArmedEscort(HttpContext, ReceivingDTO);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var type = ((ApiOkResponse)response).Result;
            return Ok(type);
        }

        [HttpPost("AddNewProfileTie")]
        public async Task<ApiCommonResponse> AddNewProfileTie(ArmedEscortSMORoutesResourceTieReceivingDTO ReceivingDTO)
        {
            var response = await _armedEscortService.AddArmedEscortTie(HttpContext, ReceivingDTO);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var type = ((ApiOkResponse)response).Result;
            return Ok(type);
        }

        [HttpPut("UpdateProfileById/{id}")]
        public async Task<IActionResult> UpdateProfileById(long id, ArmedEscortProfileReceivingDTO Receiving)
        {
            var response = await _armedEscortService.UpdateArmedEscort(HttpContext, id, Receiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var type = ((ApiOkResponse)response).Result;
            return Ok(type);
        }

        [HttpDelete("DeleteProfileById/{id}")]
        public async Task<ApiCommonResponse> DeleteProfileById(int id)
        {
            var response = await _armedEscortService.DeleteArmedEscort(id);
            return StatusCode(response.StatusCode);
        }

        [HttpDelete("DeleteProfileTieById/{id}")]
        public async Task<ApiCommonResponse> DeleteProfileTieById(int id)
        {
            var response = await _armedEscortService.DeleteArmedEscortTie(id);
            return StatusCode(response.StatusCode);
        }
    }
}
