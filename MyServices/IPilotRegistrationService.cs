using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices
{
    public interface IPilotRegistrationService
    {
        Task<ApiResponse> AddPilot(HttpContext context, PilotProfileReceivingDTO pilotReceivingDTO);
        Task<ApiResponse> GetAllPilot();
        Task<ApiResponse> GetPilotById(long id);
        Task<ApiResponse> UpdatePilot(HttpContext context, long id, PilotProfileReceivingDTO pilotReceivingDTO);
        Task<ApiResponse> DeletePilot(long id);

        //Tie
        Task<ApiResponse> AddPilotTie(HttpContext context, PilotSMORoutesResourceTieReceivingDTO pilotReceivingTieDTO);
        Task<ApiResponse> GetAllPilotTies();
        Task<ApiResponse> GetPilotTieById(long id);
        Task<ApiResponse> DeletePilotTie(long id);
    }
}
