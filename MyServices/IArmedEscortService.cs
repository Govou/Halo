using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices
{
    public interface IArmedEscortService
    {
        //Type
        Task<ApiResponse> AddArmedEscortType(HttpContext context, ArmedEscortTypeReceivingDTO armedEscortTypeReceivingDTO);
        Task<ApiResponse> GetAllCommanderTypes();
        Task<ApiResponse> GetArmedEscortTypeById(long id);
        Task<ApiResponse> UpdateArmedEscortType(HttpContext context, long id, ArmedEscortTypeReceivingDTO armedEscortTypeReceivingDTO);
        Task<ApiResponse> DeleteArmedEscortType(long id);

        //Rank
        Task<ApiResponse> AddArmedEscortRank(HttpContext context, ArmedEscortRankReceivingDTO armedEscortRankReceivingDTO);
        Task<ApiResponse> GetAllArmedEscortRanks();
        Task<ApiResponse> GetArmedEscortRankById(long id);
        Task<ApiResponse> UpdateArmedEscortRank(HttpContext context, long id, ArmedEscortRankReceivingDTO armedEscortRankReceivingDTO);
        Task<ApiResponse> DeleteArmedEscortRank(long id);
    }
}
