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
        public async Task<ActionResult> GetServiceType()
        {
            var response = await _ServiceTypeService.GetAllServiceType();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var ServiceType = ((ApiOkResponse)response).Result;
            return Ok(ServiceType);
        }
        [HttpGet("caption/{name}")]
        public async Task<ActionResult> GetByCaption(string name)
        {
            var response = await _ServiceTypeService.GetServiceTypeByName(name);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var ServiceType = ((ApiOkResponse)response).Result;
            return Ok(ServiceType);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var response = await _ServiceTypeService.GetServiceTypeById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var ServiceType = ((ApiOkResponse)response).Result;
            return Ok(ServiceType);
        }

        [HttpPost("")]
        public async Task<ActionResult> AddNewServiceType(ServiceTypeReceivingDTO ServiceTypeReceiving)
        {
            var response = await _ServiceTypeService.AddServiceType(HttpContext, ServiceTypeReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var ServiceType = ((ApiOkResponse)response).Result;
            return Ok(ServiceType);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, ServiceTypeReceivingDTO ServiceTypeReceiving)
        {
            var response = await _ServiceTypeService.UpdateServiceType(HttpContext, id, ServiceTypeReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var ServiceType = ((ApiOkResponse)response).Result;
            return Ok(ServiceType);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            var response = await _ServiceTypeService.DeleteServiceType(id);
            return StatusCode(response.StatusCode);
        }
    }
}
