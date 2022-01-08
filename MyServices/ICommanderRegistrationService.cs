using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices
{
    public interface ICommanderRegistrationService
    {
        Task<ApiCommonResponse> AddCommander(HttpContext context, CommanderProfileReceivingDTO commanderReceivingDTO);
        Task<ApiCommonResponse> GetAllCommanders();
        Task<ApiCommonResponse> GetCommanderById(long id);
        Task<ApiCommonResponse> UpdateCommander(HttpContext context, long id, CommanderProfileReceivingDTO commanderReceivingDTO);
        Task<ApiCommonResponse> DeleteCommander(long id);

        //Tie
        Task<ApiCommonResponse> AddCommanderTie(HttpContext context, CommanderSMORoutesResourceTieReceivingDTO commanderReceivingDTO);
        Task<ApiCommonResponse> GetAllCommanderTies();
        Task<ApiCommonResponse> GetCommanderTieById(long id);
        Task<ApiCommonResponse> DeleteCommanderTie(long id);
        Task<ApiResponse> AddCommanderTie(HttpContext context, CommanderSMORoutesResourceTieReceivingDTO commanderReceivingDTO);
        Task<ApiResponse> GetAllCommanderTies();
        Task<ApiResponse> GetAllCommanderTiesByResourceId(long resourceId);
        Task<ApiResponse> GetCommanderTieById(long id);
        Task<ApiResponse> DeleteCommanderTie(long id);
    }
}
