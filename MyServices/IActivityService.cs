using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices
{
    public interface IActivityService
    {
        Task<ApiResponse> AddActivity(HttpContext context, ActivityReceivingDTO serviceTypeReceivingDTO);
        Task<ApiResponse> GetAllActivity();
        Task<ApiResponse> GetActivityById(long id);
        Task<ApiResponse> GetActivityByName(string name);
        Task<ApiResponse> UpdateActivity(HttpContext context, long id, ActivityReceivingDTO serviceTypeReceivingDTO);
        Task<ApiResponse> DeleteActivity(long id);
    }
}
