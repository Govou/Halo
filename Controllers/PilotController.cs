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
        public async Task<ActionResult> GetAllPilotRanks()
        {
            var response = await _pilotService.GetAllPilotRanks();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var res = ((ApiOkResponse)response).Result;
            return Ok(res);
        }

        [HttpGet("GetAllPilotTypes")]
        public async Task<ActionResult> GetAllPilotTypes()
        {
            var response = await _pilotService.GetAllPilotTypes();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var res = ((ApiOkResponse)response).Result;
            return Ok(res);
        }

        [HttpGet("GetRankById/{id}")]
        public async Task<ActionResult> GetRankById(long id)
        {
            var response = await _pilotService.GetPilotRankById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var res = ((ApiOkResponse)response).Result;
            return Ok(res);
        }

        [HttpGet("GetTypeById/{id}")]
        public async Task<ActionResult> GetTypeById(long id)
        {
            var response = await _pilotService.GetPilotTypeById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var type = ((ApiOkResponse)response).Result;
            return Ok(type);
        }

        [HttpPost("AddNewType")]
        public async Task<ActionResult> AddNewType(PilotTypeReceivingDTO TypeReceivingDTO)
        {
            var response = await _pilotService.AddPilotType(HttpContext, TypeReceivingDTO);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var res = ((ApiOkResponse)response).Result;
            return Ok(res);
        }

        [HttpPost("AddNewRank")]
        public async Task<ActionResult> AddNewRank(PilotRankReceivingDTO RankReceivingDTO)
        {
            var response = await _pilotService.AddPilotRank(HttpContext, RankReceivingDTO);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var res = ((ApiOkResponse)response).Result;
            return Ok(res);
        }

        [HttpPut("UpdateTypeById/{id}")]
        public async Task<IActionResult> UpdateTypeById(long id, PilotTypeReceivingDTO TypeReceiving)
        {
            var response = await _pilotService.UpdatePilotType(HttpContext, id, TypeReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var res = ((ApiOkResponse)response).Result;
            return Ok(res);
        }

        [HttpPut("UpdateRankById/{id}")]
        public async Task<IActionResult> UpdateRankById(long id, PilotRankReceivingDTO RankReceiving)
        {
            var response = await _pilotService.UpdatePilotRank(HttpContext, id, RankReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var res = ((ApiOkResponse)response).Result;
            return Ok(res);
        }

        [HttpDelete("DeleteRankById/{id}")]
        public async Task<ActionResult> DeleteRankById(int id)
        {
            var response = await _pilotService.DeletePilotRank(id);
            return StatusCode(response.StatusCode);
        }

        [HttpDelete("DeleteTypeById/{id}")] //{id}
        public async Task<ActionResult> DeleteTypeById(int id)
        {
            var response = await _pilotService.DeletePilotType(id);
            return StatusCode(response.StatusCode);
        }
    }
}
