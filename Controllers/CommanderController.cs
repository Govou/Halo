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
    public class CommanderController : ControllerBase
    {
        private readonly ICommanderService _commanderService;

        public CommanderController(ICommanderService commanderService)
        {
            _commanderService = commanderService;
        }

        [HttpGet("GetAllCommanderRanks")]
        public async Task<ApiCommonResponse> GetAllCommanderRanks()
        {
            var response = await _commanderService.GetAllCommanderRanks();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var cRank = ((ApiOkResponse)response).Result;
            return Ok(cRank);
        }

        [HttpGet("GetAllCommanderTypes")]
        public async Task<ApiCommonResponse> GetAllCommanderTypes()
        {
            var response = await _commanderService.GetAllCommanderTypes();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var cType = ((ApiOkResponse)response).Result;
            return Ok(cType);
        }

        [HttpGet("GetRankById/{id}")]
        public async Task<ApiCommonResponse> GetRankById(long id)
        {
            var response = await _commanderService.GetCommanderRankById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Rank = ((ApiOkResponse)response).Result;
            return Ok(Rank);
        }

        [HttpGet("GetTypeById/{id}")]
        public async Task<ApiCommonResponse> GetTypeById(long id)
        {
            var response = await _commanderService.GetCommanderTypeById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var type = ((ApiOkResponse)response).Result;
            return Ok(type);
        }

        [HttpPost("AddNewCommanderType")]
        public async Task<ApiCommonResponse> AddNewCommanderType(CommanderTypeAndRankReceivingDTO TypeReceivingDTO)
        {
            var response = await _commanderService.AddCommanderType(HttpContext, TypeReceivingDTO);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var type = ((ApiOkResponse)response).Result;
            return Ok(type);
        }

        [HttpPost("AddNewCommanderRank")]
        public async Task<ApiCommonResponse> AddNewCommanderRank(CommanderRankReceivingDTO RankReceivingDTO)
        {
            var response = await _commanderService.AddCommanderRank(HttpContext, RankReceivingDTO);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var rank = ((ApiOkResponse)response).Result;
            return Ok(rank);
        }

        [HttpPut("UpdateTypeById/{id}")] //{id}
        public async Task<IActionResult> UpdateTypeById(long id, CommanderTypeAndRankReceivingDTO TypeReceiving)
        {
            var response = await _commanderService.UpdateCommanderType(HttpContext, id, TypeReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var type = ((ApiOkResponse)response).Result;
            return Ok(type);
        }

        [HttpPut("UpdateRankById/{id}")]
        public async Task<IActionResult> UpdateRankById(long id, CommanderRankReceivingDTO RankReceiving)
        {
            var response = await _commanderService.UpdateCommanderRank(HttpContext, id, RankReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var rank = ((ApiOkResponse)response).Result;
            return Ok(rank);
        }

        [HttpDelete("DeleterankById/{id}")]
        public async Task<ApiCommonResponse> DeleteRankById(int id)
        {
            var response = await _commanderService.DeleteCommanderRank(id);
            return StatusCode(response.StatusCode);
        }

        [HttpDelete("DeleteTypeById/{id}")] //{id}
        public async Task<ApiCommonResponse> DeleteTypeById(int id)
        {
            var response = await _commanderService.DeleteCommanderType(id);
            return StatusCode(response.StatusCode);
        }
    }
}
