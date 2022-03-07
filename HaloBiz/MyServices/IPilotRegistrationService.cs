using Halobiz.Common.DTOs.ApiDTOs;
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
        Task<ApiCommonResponse> AddPilot(HttpContext context, PilotProfileReceivingDTO pilotReceivingDTO);
        Task<ApiCommonResponse> GetAllPilot();
        Task<ApiCommonResponse> GetPilotById(long id);
        Task<ApiCommonResponse> UpdatePilot(HttpContext context, long id, PilotProfileReceivingDTO pilotReceivingDTO);
        Task<ApiCommonResponse> DeletePilot(long id);

        //Tie
        Task<ApiCommonResponse> AddPilotTie(HttpContext context, PilotSMORoutesResourceTieReceivingDTO pilotReceivingTieDTO);
        Task<ApiCommonResponse> GetAllPilotTies();
        Task<ApiCommonResponse> GetPilotTieById(long id);
        Task<ApiCommonResponse> DeletePilotTie(long id);
        Task<ApiCommonResponse> GetAllPilotTiesByResourceId(long resourceId);
    }
}
