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
    public class ArmedEscortController : ControllerBase
    {
        private readonly IArmedEscortService _armedEscortService;

        public ArmedEscortController(IArmedEscortService armedEscortService)
        {
            _armedEscortService = armedEscortService;
        }

        [HttpGet("GetAllArmedEscortRanks")]
        public async Task<ActionResult> GetAllArmedEscortRanks()
        {
            var response = await _armedEscortService.GetAllArmedEscortRanks();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var cRank = ((ApiOkResponse)response).Result;
            return Ok(cRank);
        }

        [HttpGet("GetAllArmedEscortTypes")]
        public async Task<ActionResult> GetAllArmedEscortTypes()
        {
            var response = await _armedEscortService.GetAllCommanderTypes();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var cType = ((ApiOkResponse)response).Result;
            return Ok(cType);
        }

        [HttpGet("GetRankById/{id}")]
        public async Task<ActionResult> GetRankById(long id)
        {
            var response = await _armedEscortService.GetArmedEscortRankById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Rank = ((ApiOkResponse)response).Result;
            return Ok(Rank);
        }

        [HttpGet("GetTypeById/{id}")]
        public async Task<ActionResult> GetTypeById(long id)
        {
            var response = await _armedEscortService.GetArmedEscortTypeById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var type = ((ApiOkResponse)response).Result;
            return Ok(type);
        }

        [HttpPost("AddNewType")]
        public async Task<ActionResult> AddNewType(ArmedEscortTypeReceivingDTO TypeReceivingDTO)
        {
            var response = await _armedEscortService.AddArmedEscortType(HttpContext, TypeReceivingDTO);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var type = ((ApiOkResponse)response).Result;
            return Ok(type);
        }

        [HttpPost("AddNewRank")]
        public async Task<ActionResult> AddNewRank(ArmedEscortRankReceivingDTO RankReceivingDTO)
        {
            var response = await _armedEscortService.AddArmedEscortRank(HttpContext, RankReceivingDTO);

            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var rank = ((ApiOkResponse)response).Result;
            return Ok(rank);
        }

        [HttpPut("UpdateTypeById/{id}")] 
        public async Task<IActionResult> UpdateTypeById(long id, ArmedEscortTypeReceivingDTO TypeReceiving)
        {
            var response = await _armedEscortService.UpdateArmedEscortType(HttpContext, id, TypeReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var type = ((ApiOkResponse)response).Result;
            return Ok(type);
        }

        [HttpPut("UpdateRankById/{id}")]
        public async Task<IActionResult> UpdateRankById(long id, ArmedEscortRankReceivingDTO RankReceiving)
        {
            var response = await _armedEscortService.UpdateArmedEscortRank(HttpContext, id, RankReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var rank = ((ApiOkResponse)response).Result;
            return Ok(rank);
        }

        [HttpDelete("DeleterankById/{id}")]
        public async Task<ActionResult> DeleteRankById(int id)
        {
            var response = await _armedEscortService.DeleteArmedEscortRank(id);
            return StatusCode(response.StatusCode);
        }

        [HttpDelete("DeleteTypeById/{id}")] //{id}
        public async Task<ActionResult> DeleteTypeById(int id)
        {
            var response = await _armedEscortService.DeleteArmedEscortType(id);
            return StatusCode(response.StatusCode);
        }

    }
}
