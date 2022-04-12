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
    [ModuleName(HalobizModules.SecuredMobility)]

    public class ArmedEscortController : ControllerBase
    {
        private readonly IArmedEscortService _armedEscortService;

        public ArmedEscortController(IArmedEscortService armedEscortService)
        {
            _armedEscortService = armedEscortService;
        }

        [HttpGet("GetAllArmedEscortRanks")]
        public async Task<ApiCommonResponse> GetAllArmedEscortRanks()
        {
            return await _armedEscortService.GetAllArmedEscortRanks();
        }

        [HttpGet("GetAllArmedEscortTypes")]
        public async Task<ApiCommonResponse> GetAllArmedEscortTypes()
        {
            return await _armedEscortService.GetAllArmedEscortTypes();
        }

        [HttpGet("GetRankById/{id}")]
        public async Task<ApiCommonResponse> GetRankById(long id)
        {
            return await _armedEscortService.GetArmedEscortRankById(id);
        }

        [HttpGet("GetTypeById/{id}")]
        public async Task<ApiCommonResponse> GetTypeById(long id)
        {
            return await _armedEscortService.GetArmedEscortTypeById(id);
        }

        [HttpPost("AddNewType")]
        public async Task<ApiCommonResponse> AddNewType(ArmedEscortTypeReceivingDTO TypeReceivingDTO)
        {
            return await _armedEscortService.AddArmedEscortType(HttpContext, TypeReceivingDTO);
        }

        [HttpPost("AddNewRank")]
        public async Task<ApiCommonResponse> AddNewRank(ArmedEscortRankReceivingDTO RankReceivingDTO)
        {
            return await _armedEscortService.AddArmedEscortRank(HttpContext, RankReceivingDTO);
        }

        [HttpPut("UpdateTypeById/{id}")] 
        public async Task<ApiCommonResponse> UpdateTypeById(long id, ArmedEscortTypeReceivingDTO TypeReceiving)
        {
            return await _armedEscortService.UpdateArmedEscortType(HttpContext, id, TypeReceiving);
        }

        [HttpPut("UpdateRankById/{id}")]
        public async Task<ApiCommonResponse> UpdateRankById(long id, ArmedEscortRankReceivingDTO RankReceiving)
        {
            return await _armedEscortService.UpdateArmedEscortRank(HttpContext, id, RankReceiving);
        }

        [HttpDelete("DeleterankById/{id}")]
        public async Task<ApiCommonResponse> DeleteRankById(int id)
        {
            return await _armedEscortService.DeleteArmedEscortRank(id);
        }

        [HttpDelete("DeleteTypeById/{id}")] //{id}
        public async Task<ApiCommonResponse> DeleteTypeById(int id)
        {
            return await _armedEscortService.DeleteArmedEscortType(id);
        }

    }
}
