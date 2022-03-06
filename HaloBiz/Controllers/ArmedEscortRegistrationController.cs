using Halobiz.Common.DTOs.ApiDTOs;
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
            return await _armedEscortService.GetAllArmedEscorts();
        }

        [HttpGet("GetAllArmedEscortTies")]
        public async Task<ApiCommonResponse> GetAllArmedEscortTies()
        {
            return await _armedEscortService.GetAllArmedEscortTies();
        }
        [HttpGet("GetAllArmedEscortTiesByResourceId")]
        public async Task<ApiCommonResponse> GetAllArmedEscortTiesByResourceId(long id)
        {
            return await _armedEscortService.GetAllArmedEscortTiesByResourceId(id);
            //if (response.StatusCode >= 400)
            //    return StatusCode(response.StatusCode, response);
            //var cRank = ((ApiOkResponse)response).Result;
            //return Ok(cRank);
        }
        [HttpGet("GetProfileById/{id}")]
        public async Task<ApiCommonResponse> GetProfileById(long id)
        {
            return await _armedEscortService.GetArmedEscortById(id);
        }

        [HttpGet("GetProfileTieById/{id}")]
        public async Task<ApiCommonResponse> GetProfileTieById(long id)
        {
            return await _armedEscortService.GetArmedEscortTieById(id);
        }

        [HttpPost("AddNewProfile")]
        public async Task<ApiCommonResponse> AddNewProfile(ArmedEscortProfileReceivingDTO ReceivingDTO)
        {
            return await _armedEscortService.AddArmedEscort(HttpContext, ReceivingDTO);
        }

        [HttpPost("AddNewProfileTie")]
        public async Task<ApiCommonResponse> AddNewProfileTie(ArmedEscortSMORoutesResourceTieReceivingDTO ReceivingDTO)
        {
            return await _armedEscortService.AddArmedEscortTie(HttpContext, ReceivingDTO);
        }

        [HttpPut("UpdateProfileById/{id}")]
        public async Task<ApiCommonResponse> UpdateProfileById(long id, ArmedEscortProfileReceivingDTO Receiving)
        {
            return await _armedEscortService.UpdateArmedEscort(HttpContext, id, Receiving);
        }

        [HttpDelete("DeleteProfileById/{id}")]
        public async Task<ApiCommonResponse> DeleteProfileById(int id)
        {
            return await _armedEscortService.DeleteArmedEscort(id);
        }

        [HttpDelete("DeleteProfileTieById/{id}")]
        public async Task<ApiCommonResponse> DeleteProfileTieById(int id)
        {
            return await _armedEscortService.DeleteArmedEscortTie(id);
        }
    }
}
