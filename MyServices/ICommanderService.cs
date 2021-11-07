using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices
{
    public interface ICommanderService
    {
        //Type
        Task<ApiResponse> AddCommanderType(HttpContext context, CommanderTypeAndRankReceivingDTO commanderTypeReceivingDTO);
        Task<ApiResponse> GetAllCommanderTypes();
        Task<ApiResponse> GetCommanderTypeById(long id);
        Task<ApiResponse> UpdateCommanderType(HttpContext context, long id, CommanderTypeAndRankReceivingDTO commanderTypeReceivingDTO);
        Task<ApiResponse> DeleteCommanderType(long id);

        //Rank
        Task<ApiResponse> AddCommanderRank(HttpContext context, CommanderRankReceivingDTO commanderRankReceivingDTO);
        Task<ApiResponse> GetAllCommanderRanks();
        Task<ApiResponse> GetCommanderRankById(long id);
        Task<ApiResponse> UpdateCommanderRank(HttpContext context, long id, CommanderRankReceivingDTO commanderRankReceivingDTO);
        Task<ApiResponse> DeleteCommanderRank(long id);
    }
}
