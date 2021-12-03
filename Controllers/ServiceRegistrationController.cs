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
        [HttpGet("GetAllArmedEscortResourceRequired")]
        public async Task<ActionResult> GetAllArmedEscortResourceRequired()
        {
            var response = await _serviceRegistration.GetAllArmedEscortResourceRequired();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var resource = ((ApiOkResponse)response).Result;
            return Ok(resource);
        }
        [HttpGet("GetAllPilotResourceRequired")] 
        public async Task<ActionResult> GetAllPilotResourceRequired()
        {
            var response = await _serviceRegistration.GetAllPilotResourceRequired();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var resource = ((ApiOkResponse)response).Result;
            return Ok(resource);
        }
        [HttpGet("GetAllCommanderResourceRequired")]
        public async Task<ActionResult> GetAllCommanderResourceRequired()
        {
            var response = await _serviceRegistration.GetAllCommanderResourceRequired();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var resource = ((ApiOkResponse)response).Result;
            return Ok(resource);
        }
        [HttpGet("GetAllVehicleResourceRequired")]
        public async Task<ActionResult> GetAllVehicleResourceRequired()
        {
            var response = await _serviceRegistration.GetAllVehicleResourceRequired();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var resource = ((ApiOkResponse)response).Result;
            return Ok(resource);
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
        [HttpGet("GetArmedEscortResourceRequiredById/{id}")]
        public async Task<ActionResult> GetArmedEscortResourceRequiredById(long id)
        {
            var response = await _serviceRegistration.GetArmedEscortResourceById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Rank = ((ApiOkResponse)response).Result;
            return Ok(Rank);
        }
        [HttpGet("GetPilotResourceRequiredById/{id}")] 
        public async Task<ActionResult> GetPilotResourceRequiredById(long id)
        {
            var response = await _serviceRegistration.GetPilotResourceById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Rank = ((ApiOkResponse)response).Result;
            return Ok(Rank);
        }
        [HttpGet("GetCommanderResourceRequiredById/{id}")]
        public async Task<ActionResult> GetCommanderResourceRequiredById(long id)
        {
            var response = await _serviceRegistration.GetCommanderResourceById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Rank = ((ApiOkResponse)response).Result;
            return Ok(Rank);
        }
        [HttpGet("GetVehicleResourceRequiredById/{id}")]
        public async Task<ActionResult> GetVehicleResourceRequiredById(long id)
        {
            var response = await _serviceRegistration.GetVehicleResourceById(id);
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

        [HttpPost("AddNewResourceRequired")]
        public async Task<ActionResult> AddNewResourceRequiredTypes(AllResourceTypesPerServiceRegReceivingDTO ReceivingDTO)
        {
            var response = await _serviceRegistration.AddResourceRequired(HttpContext, ReceivingDTO);
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
        [HttpDelete("DeleteArmedEscortResourceRequiredById/{id}")]
        public async Task<ActionResult> DeleteArmedEscortResourceRequiredById(int id)
        {
            var response = await _serviceRegistration.DeleteArmedEscortResource(id);
            return StatusCode(response.StatusCode);
        }
        [HttpDelete("DeletePilotResourceRequiredById/{id}")] 
        public async Task<ActionResult> DeletePilotResourceRequiredById(int id)
        {
            var response = await _serviceRegistration.DeletePilotResource(id);
            return StatusCode(response.StatusCode);
        }
        [HttpDelete("DeleteCommanderResourceRequiredById/{id}")]
        public async Task<ActionResult> DeleteCommanderResourceRequiredById(int id)
        {
            var response = await _serviceRegistration.DeleteCommanderResource(id);
            return StatusCode(response.StatusCode);
        }
        [HttpDelete("DeleteVehicleResourceRequiredById/{id}")]
        public async Task<ActionResult> DeleteVehicleResourceRequiredById(int id)
        {
            var response = await _serviceRegistration.DeleteVehicleResource(id);
            return StatusCode(response.StatusCode);
        }


    }
}
