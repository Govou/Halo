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
    public class CommanderRegistrationController : ControllerBase
    {
        private readonly ICommanderRegistrationService _commanderService;

        public CommanderRegistrationController(ICommanderRegistrationService commanderService)
        {
            _commanderService = commanderService;
        }

        [HttpGet("GetAllCommanderProfiles")]
        public async Task<ApiCommonResponse> GetAllCommanderProfiles()
        {
            return await _commanderService.GetAllCommanders();
        }
        [HttpGet("GetAllCommanderTies")]
        public async Task<ApiCommonResponse> GetAllCommanderTies()
        {
            return await _commanderService.GetAllCommanderTies();
        }
        [HttpGet("GetAllCommanderTiesByResourceId")]
        public async Task<ApiCommonResponse> GetAllCommanderTiesByResourceId(long id)
        {
            return await _commanderService.GetAllCommanderTiesByResourceId(id);
            //if (response.StatusCode >= 400)
            //    return StatusCode(response.StatusCode, response);
            //var cRank = ((ApiOkResponse)response).Result;
            //return Ok(cRank);
        }

        [HttpGet("GetProfileById/{id}")]
        public async Task<ApiCommonResponse> GetProfileById(long id)
        {
            return await _commanderService.GetCommanderById(id);
        }
        [HttpGet("GetProfileTieById/{id}")]
        public async Task<ApiCommonResponse> GetProfileTieById(long id)
        {
            return await _commanderService.GetCommanderTieById(id);
        }

        [HttpPost("AddNewCommanderProfile")]
        public async Task<ApiCommonResponse> AddNewCommanderProfile(CommanderProfileReceivingDTO ReceivingDTO)
        {
            return await _commanderService.AddCommander(HttpContext, ReceivingDTO);
        }

        [HttpPost("AddNewProfileTie")]
        public async Task<ApiCommonResponse> AddNewProfileTie(CommanderSMORoutesResourceTieReceivingDTO ReceivingDTO)
        {
            return await _commanderService.AddCommanderTie(HttpContext, ReceivingDTO);
        }

        [HttpPut("UpdateProfileById/{id}")]
        public async Task<ApiCommonResponse> UpdateProfileById(long id, CommanderProfileReceivingDTO ReceivingDTO)
        {
            return await _commanderService.UpdateCommander(HttpContext, id, ReceivingDTO);
        }

        [HttpDelete("DeleteProfileById/{id}")]
        public async Task<ApiCommonResponse> DeleteRankById(int id)
        {
            return await _commanderService.DeleteCommander(id);
        }
        [HttpDelete("DeleteProfileTieById/{id}")]
        public async Task<ApiCommonResponse> DeleteProfileTieById(int id)
        {
            return await _commanderService.DeleteCommanderTie(id);
        }
    }
}
