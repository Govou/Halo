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
    [ModuleName(HalobizModules.SecuredMobility, 125)]
    public class OnlineLocationFavoriteController : ControllerBase
    {
        private readonly IOnlineLocationFavoriteService  _onlineLocationFavoriteService;

        public OnlineLocationFavoriteController(IOnlineLocationFavoriteService onlineLocationFavoriteService)
        {
            _onlineLocationFavoriteService = onlineLocationFavoriteService;
        }

        [HttpGet("GetAllOnlineLocationFavorites")]
        public async Task<ApiCommonResponse> GetAllOnlineLocationFavorites()
        {
            return await _onlineLocationFavoriteService.GetAllOnlineLocationFavorites();
        }


        [HttpGet("GetAllOnlineLocationFavoritesByClientId/{clientId}")]
        public async Task<ApiCommonResponse> GetAllOnlineLocationFavoritesByClientId(long clientId)
        {
            return await _onlineLocationFavoriteService.GetOnlineLocationFavoriteByClientId(clientId);
        }

        [HttpGet("GetOnlineLocationFavoritesById/{id}")]
        public async Task<ApiCommonResponse> GetOnlineLocationFavoritesById(long id)
        {
            return await _onlineLocationFavoriteService.GetOnlineLocationFavoriteById(id);
        }

        [HttpPost("AddNewLocationFavorite")]
        public async Task<ApiCommonResponse> AddNewLocationFavorite(OnlineLocationFavoriteReceivingDTO ReceivingDTO)
        {
            return await _onlineLocationFavoriteService.AddOnlineLocationFavorite(HttpContext, ReceivingDTO);
        }

        [HttpPut("UpdateLocationFavoriteById/{id}")]
        public async Task<ApiCommonResponse> UpdateLocationFavoriteById(long id, OnlineLocationFavoriteReceivingDTO Receiving)
        {
            return await _onlineLocationFavoriteService.UpdateOnlineLocationFavorite(HttpContext, id, Receiving);
        }

        [HttpDelete("DeleteLocationFavoriteById/{id}")]
        public async Task<ApiCommonResponse> DeleteLocationFavoriteById(int id)
        {
            return await _onlineLocationFavoriteService.DeleteOnlineLocationFavorite(id);
        }
    }
}
