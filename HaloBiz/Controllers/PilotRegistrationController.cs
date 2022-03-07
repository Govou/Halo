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
    public class PilotRegistrationController : ControllerBase
    {
        private readonly IPilotRegistrationService _pilotService;

        public PilotRegistrationController(IPilotRegistrationService pilotService)
        {
            _pilotService = pilotService;
        }

        [HttpGet("GetAllPilotProfiles")]
        public async Task<ApiCommonResponse> GetAllPilotProfiles()
        {
            return await _pilotService.GetAllPilot();
        }

        [HttpGet("GetAllPilotTies")]
        public async Task<ApiCommonResponse> GetAllPilotTies()
        {
            return await _pilotService.GetAllPilotTies();
        }

        [HttpGet("GetAllPilotTiesByResourceId")]
        public async Task<ApiCommonResponse> GetAllPilotTiesByResourceId(long id)
        {
            return await _pilotService.GetAllPilotTiesByResourceId(id);
            //if (response.StatusCode >= 400)
            //    return StatusCode(response.StatusCode, response);
            //var res = ((ApiOkResponse)response).Result;
            //return Ok(res);
        }


        [HttpGet("GetProfileById/{id}")]
        public async Task<ApiCommonResponse> GetProfileById(long id)
        {
            return await _pilotService.GetPilotById(id);
        }

        [HttpGet("GetProfileTieById/{id}")]
        public async Task<ApiCommonResponse> GetProfileTieById(long id)
        {
            return await _pilotService.GetPilotTieById(id);
        }

        [HttpPost("AddNewProfile")]
        public async Task<ApiCommonResponse> AddNewProfile(PilotProfileReceivingDTO pilotReceivingDTO)
        {
            return await _pilotService.AddPilot(HttpContext, pilotReceivingDTO);
        }

        [HttpPost("AddNewProfileTie")]
        public async Task<ApiCommonResponse> AddNewProfileTie(PilotSMORoutesResourceTieReceivingDTO pilotReceivingDTO)
        {
            return await _pilotService.AddPilotTie(HttpContext, pilotReceivingDTO);
        }

        [HttpPut("UpdateProfileById/{id}")]
        public async Task<ApiCommonResponse> UpdateProfileById(long id, PilotProfileReceivingDTO pilotReceivingDTO)
        {
            return await _pilotService.UpdatePilot(HttpContext, id, pilotReceivingDTO);
        }

        [HttpDelete("DeleteProfileById/{id}")]
        public async Task<ApiCommonResponse> DeleteProfileById(int id)
        {
            return await _pilotService.DeletePilot(id);
        }

        [HttpDelete("DeleteProfileTieById/{id}")]
        public async Task<ApiCommonResponse> DeleteProfileTieById(int id)
        {
            return await _pilotService.DeletePilotTie(id);
        }
    }
}
