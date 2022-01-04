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
        Task<ApiCommonResponse> AddServiceType(HttpContext context, ServiceTypeReceivingDTO serviceTypeReceivingDTO);
        Task<ApiCommonResponse> GetAllServiceType();
        Task<ApiCommonResponse> GetServiceTypeById(long id);
        Task<ApiCommonResponse> GetServiceTypeByName(string name);
        Task<ApiCommonResponse> UpdateServiceType(HttpContext context, long id, ServiceTypeReceivingDTO serviceTypeReceivingDTO);
        Task<ApiCommonResponse> DeleteServiceType(long id);
    }
}
