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
    public class ServiceTypeController : ControllerBase
    {
        private readonly IServiceTypeService _ServiceTypeService;

        public ServiceTypeController(IServiceTypeService serviceTypeService)
        {
            this._ServiceTypeService = serviceTypeService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetServiceType()
        {
            return await _ServiceTypeService.GetAllServiceType();
        }
        [HttpGet("caption/{name}")]
        public async Task<ApiCommonResponse> GetByCaption(string name)
        {
            return await _ServiceTypeService.GetServiceTypeByName(name);
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _ServiceTypeService.GetServiceTypeById(id);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewServiceType(ServiceTypeReceivingDTO ServiceTypeReceiving)
        {
            return await _ServiceTypeService.AddServiceType(HttpContext, ServiceTypeReceiving);
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, ServiceTypeReceivingDTO ServiceTypeReceiving)
        {
            return await _ServiceTypeService.UpdateServiceType(HttpContext, id, ServiceTypeReceiving); 
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _ServiceTypeService.DeleteServiceType(id);
        }
    }
}
