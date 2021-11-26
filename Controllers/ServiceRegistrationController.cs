using HaloBiz.DTOs;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.MyServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ServiceRegistrationController : ControllerBase
    {
        private readonly IServiceRegistrationService _serviceRegistration;


        public ServiceRegistrationController(IServiceRegistrationService serviceRegistration)
        {
            _serviceRegistration = serviceRegistration;
        }


        [HttpGet("GetAllRegisteredServices")]
        public async Task<ActionResult> GetAllRegisteredServices()
        {
            var response = await _serviceRegistration.GetAllServiceRegs();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var cRank = ((ApiOkResponse)response).Result;
            return Ok(cRank);
        }

        [HttpGet("GetRegServiceById/{id}")]
        public async Task<ActionResult> GetRegServiceById(long id)
        {
            var response = await _serviceRegistration.GetServiceRegById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Rank = ((ApiOkResponse)response).Result;
            return Ok(Rank);
        }

        [HttpPost("AddNewServiceReg")]
        public async Task<ActionResult> AddNewServiceReg(ServiceRegistrationReceivingDTO ReceivingDTO)
        {
            var response = await _serviceRegistration.AddServiceReg(HttpContext, ReceivingDTO);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var type = ((ApiOkResponse)response).Result;
            return Ok(type);
        }

        [HttpPut("UpdateRegServiceById/{id}")]
        public async Task<IActionResult> UpdateRegServiceById(long id, ServiceRegistrationReceivingDTO ReceivingDTO)
        {
            var response = await _serviceRegistration.UpdateServiceReg(HttpContext, id, ReceivingDTO);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var type = ((ApiOkResponse)response).Result;
            return Ok(type);
        }

        [HttpDelete("DeleteRegServiceById/{id}")]
        public async Task<ActionResult> DeleteRegServiceById(int id)
        {
            var response = await _serviceRegistration.DeleteServiceReg(id);
            return StatusCode(response.StatusCode);
        }

        [HttpPut("UpdateVehicleTypeServiceById/{id}")]
        public async Task<IActionResult> UpdateVehicleTypeServiceById(long id, VehicleTypeReceivingDTO[] ReceivingDTO)
        {
            var response = await _serviceRegistration.AddUpVehicleType(HttpContext, id, ReceivingDTO);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var type = ((ApiOkResponse)response).Result;
            return Ok(type);
        }

        [HttpPut("UpdatePilotTypeServiceById/{id}")]
        public async Task<IActionResult> UpdatePilotTypeServiceById(long id, PilotTypeReceivingDTO[] ReceivingDTO)
        {
            var response = await _serviceRegistration.AddUpPilotType(HttpContext, id, ReceivingDTO);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var type = ((ApiOkResponse)response).Result;
            return Ok(type);
        }


        [HttpPut("UpdateCommanderTypeServiceById/{id}")]
        public async Task<IActionResult> UpdateCommanderTypeServiceById(long id, CommanderTypeAndRankReceivingDTO[] ReceivingDTO)
        {
            var response = await _serviceRegistration.AddUpCommanderType(HttpContext, id, ReceivingDTO);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var type = ((ApiOkResponse)response).Result;
            return Ok(type);
        }

        [HttpPut("UpdateArmedEscortTypeServiceById/{id}")]
        public async Task<IActionResult> UpdateArmedEscortTypeServiceById(long id, ArmedEscortTypeReceivingDTO[] ReceivingDTO)
        {
            var response = await _serviceRegistration.AddUpArmedEscortType(HttpContext, id, ReceivingDTO);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var type = ((ApiOkResponse)response).Result;
            return Ok(type);
        }
    }
}
