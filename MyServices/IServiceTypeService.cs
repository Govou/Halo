using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices
{
    public interface IServiceTypeService
    {
        Task<ApiResponse> AddServiceType(HttpContext context, ServiceTypeReceivingDTO serviceTypeReceivingDTO);
        Task<ApiResponse> GetAllServiceType();
        Task<ApiResponse> GetServiceTypeById(long id);
        Task<ApiResponse> GetServiceTypeByName(string name);
        Task<ApiResponse> UpdateServiceType(HttpContext context, long id, ServiceTypeReceivingDTO serviceTypeReceivingDTO);
        Task<ApiResponse> DeleteServiceType(long id);
    }
}
