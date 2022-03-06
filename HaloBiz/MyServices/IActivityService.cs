using Halobiz.Common.DTOs.ApiDTOs;
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
        Task<ApiCommonResponse> AddActivity(HttpContext context, ActivityReceivingDTO serviceTypeReceivingDTO);
        Task<ApiCommonResponse> GetAllActivity();
        Task<ApiCommonResponse> GetActivityById(long id);
        Task<ApiCommonResponse> GetActivityByName(string name);
        Task<ApiCommonResponse> UpdateActivity(HttpContext context, long id, ActivityReceivingDTO serviceTypeReceivingDTO);
        Task<ApiCommonResponse> DeleteActivity(long id);
    }
}
