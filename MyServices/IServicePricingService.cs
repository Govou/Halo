using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices
{
    public interface IServicePricingService
    {
        Task<ApiResponse> AddServicePricing(HttpContext context, ServicePricingReceivingDTO servicePricingReceivingDTO);
        Task<ApiResponse> GetAllServicePricing();
        Task<ApiResponse> GetServicePricingById(long id);
        Task<ApiResponse> GetServicePricingByServiceId(long serviceId);
        Task<ApiResponse> GetServicePricingByBranchId(long branchId);
        Task<ApiResponse> UpdateServicePricing(HttpContext context, long id, ServicePricingReceivingDTO servicePricingReceivingDTO);
        Task<ApiResponse> DeleteServicePricing(long id);
    }
}
