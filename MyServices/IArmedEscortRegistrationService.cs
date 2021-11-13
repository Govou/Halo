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
        Task<ApiResponse> AddArmedEscort(HttpContext context, ArmedEscortProfileReceivingDTO armedEscortReceivingDTO);
        Task<ApiResponse> GetAllArmedEscorts();
        Task<ApiResponse> GetArmedEscortById(long id);
        Task<ApiResponse> UpdateArmedEscort(HttpContext context, long id, ArmedEscortProfileReceivingDTO armedEscortReceivingDTO);
        Task<ApiResponse> DeleteArmedEscort(long id);
    }
}
