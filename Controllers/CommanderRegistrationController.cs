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
    public class CommanderRegistrationController : ControllerBase
    {
        private readonly ICommanderRegistrationService _commanderService;

        public CommanderRegistrationController(ICommanderRegistrationService commanderService)
        {
            _commanderService = commanderService;
        }

        [HttpGet("GetAllCommanderProfiles")]
        public async Task<ActionResult> GetAllCommanderProfiles()
        {
            var response = await _commanderService.GetAllCommanders();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var cRank = ((ApiOkResponse)response).Result;
            return Ok(cRank);
        }
        [HttpGet("GetAllCommanderTies")]
        public async Task<ActionResult> GetAllCommanderTies()
        {
            var response = await _commanderService.GetAllCommanderTies();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var cRank = ((ApiOkResponse)response).Result;
            return Ok(cRank);
        }

        [HttpGet("GetProfileById/{id}")]
        public async Task<ActionResult> GetProfileById(long id)
        {
            var response = await _commanderService.GetCommanderById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Rank = ((ApiOkResponse)response).Result;
            return Ok(Rank);
        }
        [HttpGet("GetProfileTieById/{id}")]
        public async Task<ActionResult> GetProfileTieById(long id)
        {
            var response = await _commanderService.GetCommanderTieById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Rank = ((ApiOkResponse)response).Result;
            return Ok(Rank);
        }

        [HttpPost("AddNewCommanderProfile")]
        public async Task<ActionResult> AddNewCommanderProfile(CommanderProfileReceivingDTO ReceivingDTO)
        {
            var response = await _commanderService.AddCommander(HttpContext, ReceivingDTO);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var type = ((ApiOkResponse)response).Result;
            return Ok(type);
        }

        [HttpPost("AddNewProfileTie")]
        public async Task<ActionResult> AddNewProfileTie(CommanderSMORoutesResourceTieReceivingDTO ReceivingDTO)
        {
            var response = await _commanderService.AddCommanderTie(HttpContext, ReceivingDTO);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var type = ((ApiOkResponse)response).Result;
            return Ok(type);
        }

        [HttpPut("UpdateProfileById/{id}")]
        public async Task<IActionResult> UpdateProfileById(long id, CommanderProfileReceivingDTO ReceivingDTO)
        {
            var response = await _commanderService.UpdateCommander(HttpContext, id, ReceivingDTO);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var rank = ((ApiOkResponse)response).Result;
            return Ok(rank);
        }

        [HttpDelete("DeleteProfileById/{id}")]
        public async Task<ActionResult> DeleteRankById(int id)
        {
            var response = await _commanderService.DeleteCommander(id);
            return StatusCode(response.StatusCode);
        }
        [HttpDelete("DeleteProfileTieById/{id}")]
        public async Task<ActionResult> DeleteProfileTieById(int id)
        {
            var response = await _commanderService.DeleteCommanderTie(id);
            return StatusCode(response.StatusCode);
        }
    }
}
