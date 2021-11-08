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
        Task<ApiResponse> AddPilotType(HttpContext context, PilotTypeReceivingDTO pilotTypeReceivingDTO);
        Task<ApiResponse> GetAllPilotTypes();
        Task<ApiResponse> GetPilotTypeById(long id);
        Task<ApiResponse> UpdatePilotType(HttpContext context, long id, PilotTypeReceivingDTO pilotTypeReceivingDTO);
        Task<ApiResponse> DeletePilotType(long id);

        //Rank
        Task<ApiResponse> AddPilotRank(HttpContext context, PilotRankReceivingDTO pilotRankReceivingDTO);
        Task<ApiResponse> GetAllPilotRanks();
        Task<ApiResponse> GetPilotRankById(long id);
        Task<ApiResponse> UpdatePilotRank(HttpContext context, long id, PilotRankReceivingDTO pilotRankReceivingDTO);
        Task<ApiResponse> DeletePilotRank(long id);
    }
}
