using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices
{
    public interface ITargetService
    {
        Task<ApiCommonResponse> AddTarget(HttpContext context, TargetReceivingDTO targetReceivingDTO);
        Task<ApiCommonResponse> GetAllTarget();
        Task<ApiCommonResponse> GetTargetById(long id);
        Task<ApiCommonResponse> GetTargetByName(string name);
        Task<ApiCommonResponse> UpdateTarget(HttpContext context, long id, TargetReceivingDTO targetReceivingDTO);
        Task<ApiCommonResponse> DeleteTarget(long id);
    }
}
