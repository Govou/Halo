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
            var response = await _ServicePricingService.GetAllServicePricing();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var ServicePricing = ((ApiOkResponse)response).Result;
            return Ok(ServicePricing);
        }

        [HttpGet("ServiceId/{serviceId}")]
        public async Task<ApiCommonResponse> GetByServiceId(long serviceId)
        {
            var response = await _ServicePricingService.GetServicePricingByServiceId(serviceId);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var ServicePricing = ((ApiOkResponse)response).Result;
            return Ok(ServicePricing);
        }

        [HttpGet("BranchId/{branchId}")]
        public async Task<ApiCommonResponse> GetByBranchId(long branchId)
        {
            var response = await _ServicePricingService.GetServicePricingByBranchId(branchId);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var ServicePricing = ((ApiOkResponse)response).Result;
            return Ok(ServicePricing);
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            var response = await _ServicePricingService.GetServicePricingById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var ServicePricing = ((ApiOkResponse)response).Result;
            return Ok(ServicePricing);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewServicePricing(ServicePricingReceivingDTO ServicePricingReceiving)
        {
            var response = await _ServicePricingService.AddServicePricing(HttpContext, ServicePricingReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var ServicePricing = ((ApiOkResponse)response).Result;
            return Ok(ServicePricing);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, ServicePricingReceivingDTO ServicePricingReceiving)
        {
            var response = await _ServicePricingService.UpdateServicePricing(HttpContext, id, ServicePricingReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var ServicePricing = ((ApiOkResponse)response).Result;
            return Ok(ServicePricing);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            var response = await _ServicePricingService.DeleteServicePricing(id);
            return StatusCode(response.StatusCode);
        }
    }
}
