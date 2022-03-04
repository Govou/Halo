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
        Task<ApiCommonResponse> AddArmedEscortType(HttpContext context, ArmedEscortTypeReceivingDTO armedEscortTypeReceivingDTO);
        Task<ApiCommonResponse> GetAllArmedEscortTypes();
        Task<ApiCommonResponse> GetArmedEscortTypeById(long id);
        Task<ApiCommonResponse> UpdateArmedEscortType(HttpContext context, long id, ArmedEscortTypeReceivingDTO armedEscortTypeReceivingDTO);
        Task<ApiCommonResponse> DeleteArmedEscortType(long id);

        //Rank
        Task<ApiCommonResponse> AddArmedEscortRank(HttpContext context, ArmedEscortRankReceivingDTO armedEscortRankReceivingDTO);
        Task<ApiCommonResponse> GetAllArmedEscortRanks();
        Task<ApiCommonResponse> GetArmedEscortRankById(long id);
        Task<ApiCommonResponse> UpdateArmedEscortRank(HttpContext context, long id, ArmedEscortRankReceivingDTO armedEscortRankReceivingDTO);
        Task<ApiCommonResponse> DeleteArmedEscortRank(long id);
    }
}
