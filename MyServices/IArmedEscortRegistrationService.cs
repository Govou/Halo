using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices
{
    public interface IArmedEscortRegistrationService
    {
        Task<ApiCommonResponse> AddArmedEscort(HttpContext context, ArmedEscortProfileReceivingDTO armedEscortReceivingDTO);
        Task<ApiCommonResponse> GetAllArmedEscorts();
        Task<ApiCommonResponse> GetArmedEscortById(long id);
        Task<ApiCommonResponse> UpdateArmedEscort(HttpContext context, long id, ArmedEscortProfileReceivingDTO armedEscortReceivingDTO);
        Task<ApiCommonResponse> DeleteArmedEscort(long id);

        //Tie
        Task<ApiCommonResponse> AddArmedEscortTie(HttpContext context, ArmedEscortSMORoutesResourceTieReceivingDTO armedEscortTieReceivingDTO);
        Task<ApiCommonResponse> GetAllArmedEscortTies();
        Task<ApiCommonResponse> GetArmedEscortTieById(long id);
        Task<ApiCommonResponse> DeleteArmedEscortTie(long id);

    }
}
