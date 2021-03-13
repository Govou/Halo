using HaloBiz.DTOs.ApiDTOs;
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
        Task<ApiResponse> AddTarget(HttpContext context, TargetReceivingDTO targetReceivingDTO);
        Task<ApiResponse> GetAllTarget();
        Task<ApiResponse> GetTargetById(long id);
        Task<ApiResponse> GetTargetByName(string name);
        Task<ApiResponse> UpdateTarget(HttpContext context, long id, TargetReceivingDTO targetReceivingDTO);
        Task<ApiResponse> DeleteTarget(long id);
    }
}
