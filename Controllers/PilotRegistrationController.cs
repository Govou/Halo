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
    public class PilotRegistrationController : ControllerBase
    {
        private readonly IPilotRegistrationService _pilotService;

        public PilotRegistrationController(IPilotRegistrationService pilotService)
        {
            _pilotService = pilotService;
        }

        [HttpGet("GetAllPilotProfiles")]
        public async Task<ActionResult> GetAllPilotProfiles()
        {
            var response = await _pilotService.GetAllPilot();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var res = ((ApiOkResponse)response).Result;
            return Ok(res);
        }

        [HttpGet("GetProfileById/{id}")]
        public async Task<ActionResult> GetProfileById(long id)
        {
            var response = await _pilotService.GetPilotById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var res = ((ApiOkResponse)response).Result;
            return Ok(res);
        }

        [HttpPost("AddNewProfile")]
        public async Task<ActionResult> AddNewProfile(PilotProfileReceivingDTO pilotReceivingDTO)
        {
            var response = await _pilotService.AddPilot(HttpContext, pilotReceivingDTO);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var res = ((ApiOkResponse)response).Result;
            return Ok(res);
        }

        [HttpPut("UpdateProfileById/{id}")]
        public async Task<IActionResult> UpdateProfileById(long id, PilotProfileReceivingDTO pilotReceivingDTO)
        {
            var response = await _pilotService.UpdatePilot(HttpContext, id, pilotReceivingDTO);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var res = ((ApiOkResponse)response).Result;
            return Ok(res);
        }

        [HttpDelete("DeleteProfileById/{id}")]
        public async Task<ActionResult> DeleteProfileById(int id)
        {
            var response = await _pilotService.DeletePilot(id);
            return StatusCode(response.StatusCode);
        }
    }
}
