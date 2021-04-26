using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices
{
    public interface IProspectService
    {
        Task<ApiResponse> AddProspect(HttpContext context, ProspectReceivingDTO prospectReceivingDTO);
        Task<ApiResponse> GetAllProspect();
        Task<ApiResponse> GetProspectById(long id);
        Task<ApiResponse> UpdateProspect(HttpContext context, long id, ProspectReceivingDTO prospectReceivingDTO);
        Task<ApiResponse> DeleteProspect(long id);
    }
}
