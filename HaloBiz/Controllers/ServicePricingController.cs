using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs.LAMS;
using HaloBiz.MyServices;
using HaloBiz.MyServices.LAMS;
using Microsoft.AspNetCore.Mvc;

namespace HaloBiz.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ServicePricingController : ControllerBase
    {
        private readonly IServicePricingService _ServicePricingService;

        public ServicePricingController(IServicePricingService servicePricingService)
        {
            this._ServicePricingService = servicePricingService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetServicePricing()
        {
            return await _ServicePricingService.GetAllServicePricing();
        }

        [HttpGet("ServiceId/{serviceId}")]
        public async Task<ApiCommonResponse> GetByServiceId(long serviceId)
        {
            return await _ServicePricingService.GetServicePricingByServiceId(serviceId);
        }

        [HttpGet("BranchId/{branchId}")]
        public async Task<ApiCommonResponse> GetByBranchId(long branchId)
        {
            return await _ServicePricingService.GetServicePricingByBranchId(branchId);
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _ServicePricingService.GetServicePricingById(id);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewServicePricing(ServicePricingReceivingDTO ServicePricingReceiving)
        {
            return await _ServicePricingService.AddServicePricing(HttpContext, ServicePricingReceiving);
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, ServicePricingReceivingDTO ServicePricingReceiving)
        {
            return await _ServicePricingService.UpdateServicePricing(HttpContext, id, ServicePricingReceiving);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _ServicePricingService.DeleteServicePricing(id);
        }
    }
}
