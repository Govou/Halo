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
        Task<ApiCommonResponse> AddCommanderType(HttpContext context, CommanderTypeAndRankReceivingDTO commanderTypeReceivingDTO);
        Task<ApiCommonResponse> GetAllCommanderTypes();
        Task<ApiCommonResponse> GetCommanderTypeById(long id);
        Task<ApiCommonResponse> UpdateCommanderType(HttpContext context, long id, CommanderTypeAndRankReceivingDTO commanderTypeReceivingDTO);
        Task<ApiCommonResponse> DeleteCommanderType(long id);

        //Rank
        Task<ApiCommonResponse> AddCommanderRank(HttpContext context, CommanderRankReceivingDTO commanderRankReceivingDTO);
        Task<ApiCommonResponse> GetAllCommanderRanks();
        Task<ApiCommonResponse> GetCommanderRankById(long id);
        Task<ApiCommonResponse> UpdateCommanderRank(HttpContext context, long id, CommanderRankReceivingDTO commanderRankReceivingDTO);
        Task<ApiCommonResponse> DeleteCommanderRank(long id);
    }
}
