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
        Task<ApiResponse> AddCommander(HttpContext context, CommanderProfileReceivingDTO commanderReceivingDTO);
        Task<ApiResponse> GetAllCommanders();
        Task<ApiResponse> GetCommanderById(long id);
        Task<ApiResponse> UpdateCommander(HttpContext context, long id, CommanderProfileReceivingDTO commanderReceivingDTO);
        Task<ApiResponse> DeleteCommander(long id);

        //Tie
        Task<ApiResponse> AddCommanderTie(HttpContext context, CommanderSMORoutesResourceTieReceivingDTO commanderReceivingDTO);
        Task<ApiResponse> GetAllCommanderTies();
        Task<ApiResponse> GetAllCommanderTiesByResourceId(long resourceId);
        Task<ApiResponse> GetCommanderTieById(long id);
        Task<ApiResponse> DeleteCommanderTie(long id);
    }
}
