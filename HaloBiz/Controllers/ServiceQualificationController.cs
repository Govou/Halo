using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs.LAMS;
using HaloBiz.MyServices;
using HaloBiz.MyServices.LAMS;
using Microsoft.AspNetCore.Mvc;

namespace HaloBiz.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ServiceQualificationController : ControllerBase
    {
        private readonly IServiceQualificationService _ServiceQualificationService;

        public ServiceQualificationController(IServiceQualificationService serviceQualificationService)
        {
            this._ServiceQualificationService = serviceQualificationService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetServiceQualification()
        {
            return await _ServiceQualificationService.GetAllServiceQualification();
        }
        /*[HttpGet("caption/{name}")]
        public async Task<ApiCommonResponse> GetByCaption(string name)
        {
            return await _ServiceQualificationService.GetServiceQualificationByName(name);
            
                
            var ServiceQualification = ((ApiOkResponse)response).Result;
            return Ok(ServiceQualification);
        }*/

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _ServiceQualificationService.GetServiceQualificationById(id);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewServiceQualification(ServiceQualificationReceivingDTO ServiceQualificationReceiving)
        {
            return await _ServiceQualificationService.AddServiceQualification(HttpContext, ServiceQualificationReceiving);
            
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, ServiceQualificationReceivingDTO ServiceQualificationReceiving)
        {
            return await _ServiceQualificationService.UpdateServiceQualification(HttpContext, id, ServiceQualificationReceiving);
        
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _ServiceQualificationService.DeleteServiceQualification(id);
        }
    }
}
