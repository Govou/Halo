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
            var response = await _ServiceQualificationService.GetAllServiceQualification();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var ServiceQualification = ((ApiOkResponse)response).Result;
            return Ok(ServiceQualification);
        }
        /*[HttpGet("caption/{name}")]
        public async Task<ApiCommonResponse> GetByCaption(string name)
        {
            var response = await _ServiceQualificationService.GetServiceQualificationByName(name);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var ServiceQualification = ((ApiOkResponse)response).Result;
            return Ok(ServiceQualification);
        }*/

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            var response = await _ServiceQualificationService.GetServiceQualificationById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var ServiceQualification = ((ApiOkResponse)response).Result;
            return Ok(ServiceQualification);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewServiceQualification(ServiceQualificationReceivingDTO ServiceQualificationReceiving)
        {
            var response = await _ServiceQualificationService.AddServiceQualification(HttpContext, ServiceQualificationReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var ServiceQualification = ((ApiOkResponse)response).Result;
            return Ok(ServiceQualification);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, ServiceQualificationReceivingDTO ServiceQualificationReceiving)
        {
            var response = await _ServiceQualificationService.UpdateServiceQualification(HttpContext, id, ServiceQualificationReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var ServiceQualification = ((ApiOkResponse)response).Result;
            return Ok(ServiceQualification);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            var response = await _ServiceQualificationService.DeleteServiceQualification(id);
            return StatusCode(response.StatusCode);
        }
    }
}
