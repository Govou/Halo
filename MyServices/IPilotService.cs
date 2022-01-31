using HaloBiz.DTOs;
using HaloBiz.DTOs.ApiDTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices
{
    public interface IPilotService
    {
        //Type
        Task<ApiCommonResponse> AddPilotType(HttpContext context, PilotTypeReceivingDTO pilotTypeReceivingDTO);
        Task<ApiCommonResponse> GetAllPilotTypes();
        Task<ApiCommonResponse> GetPilotTypeById(long id);
        Task<ApiCommonResponse> UpdatePilotType(HttpContext context, long id, PilotTypeReceivingDTO pilotTypeReceivingDTO);
        Task<ApiCommonResponse> DeletePilotType(long id);

        //Rank
        Task<ApiCommonResponse> AddPilotRank(HttpContext context, PilotRankReceivingDTO pilotRankReceivingDTO);
        Task<ApiCommonResponse> GetAllPilotRanks();
        Task<ApiCommonResponse> GetPilotRankById(long id);
        Task<ApiCommonResponse> UpdatePilotRank(HttpContext context, long id, PilotRankReceivingDTO pilotRankReceivingDTO);
        Task<ApiCommonResponse> DeletePilotRank(long id);
    }
}
