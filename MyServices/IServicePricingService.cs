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
        Task<ApiCommonResponse> AddServicePricing(HttpContext context, ServicePricingReceivingDTO servicePricingReceivingDTO);
        Task<ApiCommonResponse> GetAllServicePricing();
        Task<ApiCommonResponse> GetServicePricingById(long id);
        Task<ApiCommonResponse> GetServicePricingByServiceId(long serviceId);
        Task<ApiCommonResponse> GetServicePricingByBranchId(long branchId);
        Task<ApiCommonResponse> UpdateServicePricing(HttpContext context, long id, ServicePricingReceivingDTO servicePricingReceivingDTO);
        Task<ApiCommonResponse> DeleteServicePricing(long id);
    }
}
