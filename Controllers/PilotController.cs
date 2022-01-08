using HaloBiz.DTOs;
using HaloBiz.DTOs.ApiDTOs;
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
    public class PilotController : ControllerBase
    {
        private readonly IPilotService _pilotService;

        public PilotController(IPilotService pilotService)
        {
            _pilotService = pilotService;
        }

        [HttpGet("GetAllPilotRanks")]
        public async Task<ApiCommonResponse> GetAllPilotRanks()
        {
            return await _pilotService.GetAllPilotRanks();
        }

        [HttpGet("GetAllPilotTypes")]
        public async Task<ApiCommonResponse> GetAllPilotTypes()
        {
            return await _pilotService.GetAllPilotTypes();
        }

        [HttpGet("GetRankById/{id}")]
        public async Task<ApiCommonResponse> GetRankById(long id)
        {
            return await _pilotService.GetPilotRankById(id);
        }

        [HttpGet("GetTypeById/{id}")]
        public async Task<ApiCommonResponse> GetTypeById(long id)
        {
            return await _pilotService.GetPilotTypeById(id);
        }

        [HttpPost("AddNewType")]
        public async Task<ApiCommonResponse> AddNewType(PilotTypeReceivingDTO TypeReceivingDTO)
        {
            return await _pilotService.AddPilotType(HttpContext, TypeReceivingDTO);
        }

        [HttpPost("AddNewRank")]
        public async Task<ApiCommonResponse> AddNewRank(PilotRankReceivingDTO RankReceivingDTO)
        {
            return await _pilotService.AddPilotRank(HttpContext, RankReceivingDTO);
        }

        [HttpPut("UpdateTypeById/{id}")]
        public async Task<ApiCommonResponse> UpdateTypeById(long id, PilotTypeReceivingDTO TypeReceiving)
        {
            return await _pilotService.UpdatePilotType(HttpContext, id, TypeReceiving);
        }

        [HttpPut("UpdateRankById/{id}")]
        public async Task<ApiCommonResponse> UpdateRankById(long id, PilotRankReceivingDTO RankReceiving)
        {
            return await _pilotService.UpdatePilotRank(HttpContext, id, RankReceiving);
        }

        [HttpDelete("DeleteRankById/{id}")]
        public async Task<ApiCommonResponse> DeleteRankById(int id)
        {
            return await _pilotService.DeletePilotRank(id);
        }

        [HttpDelete("DeleteTypeById/{id}")] //{id}
        public async Task<ApiCommonResponse> DeleteTypeById(int id)
        {
            return await _pilotService.DeletePilotType(id);
        }
    }
}
