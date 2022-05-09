using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices
{
    public interface IOnlineLocationFavoriteService
    {
        Task<ApiCommonResponse> AddOnlineLocationFavorite(HttpContext context, OnlineLocationFavoriteReceivingDTO favoriteReceivingDTO);
        Task<ApiCommonResponse> GetAllOnlineLocationFavorites();
        Task<ApiCommonResponse> GetOnlineLocationFavoriteById(long id);
        Task<ApiCommonResponse> GetOnlineLocationFavoriteByClientId(long clientId);
        Task<ApiCommonResponse> UpdateOnlineLocationFavorite(HttpContext context, long id, OnlineLocationFavoriteReceivingDTO favoriteReceivingDTO);
        Task<ApiCommonResponse> DeleteOnlineLocationFavorite(long id);
    }
}
