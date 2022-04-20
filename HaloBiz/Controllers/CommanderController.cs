using Halobiz.Common.Auths;
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
    [ModuleName(HalobizModules.SecuredMobility,49)]

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
            return await _commanderService.GetAllCommanderRanks();
        }

        [HttpGet("GetAllCommanderTypes")]
        public async Task<ApiCommonResponse> GetAllCommanderTypes()
        {
            return await _commanderService.GetAllCommanderTypes();
        }

        [HttpGet("GetRankById/{id}")]
        public async Task<ApiCommonResponse> GetRankById(long id)
        {
            return await _commanderService.GetCommanderRankById(id);
        }

        [HttpGet("GetTypeById/{id}")]
        public async Task<ApiCommonResponse> GetTypeById(long id)
        {
            return await _commanderService.GetCommanderTypeById(id);
        }

        [HttpPost("AddNewCommanderType")]
        public async Task<ApiCommonResponse> AddNewCommanderType(CommanderTypeAndRankReceivingDTO TypeReceivingDTO)
        {
                return await _commanderService.AddCommanderType(HttpContext, TypeReceivingDTO);
        }

        [HttpPost("AddNewCommanderRank")]
        public async Task<ApiCommonResponse> AddNewCommanderRank(CommanderRankReceivingDTO RankReceivingDTO)
        {
            return await _commanderService.AddCommanderRank(HttpContext, RankReceivingDTO);
        }

        [HttpPut("UpdateTypeById/{id}")] //{id}
        public async Task<ApiCommonResponse> UpdateTypeById(long id, CommanderTypeAndRankReceivingDTO TypeReceiving)
        {
            return await _commanderService.UpdateCommanderType(HttpContext, id, TypeReceiving);
        }

        [HttpPut("UpdateRankById/{id}")]
        public async Task<ApiCommonResponse> UpdateRankById(long id, CommanderRankReceivingDTO RankReceiving)
        {
            return await _commanderService.UpdateCommanderRank(HttpContext, id, RankReceiving);
        }

        [HttpDelete("DeleterankById/{id}")]
        public async Task<ApiCommonResponse> DeleteRankById(int id)
        {
            return await _commanderService.DeleteCommanderRank(id);
        }

        [HttpDelete("DeleteTypeById/{id}")] //{id}
        public async Task<ApiCommonResponse> DeleteTypeById(int id)
        {
            return await _commanderService.DeleteCommanderType(id);
        }
    }
}
