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
    public class VehicleRegistrationController : ControllerBase
    {
        private readonly IVehicleRegistrationService _vehicleService;

        public VehicleRegistrationController(IVehicleRegistrationService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [HttpGet("GetAllVehicles")]
        public async Task<ActionResult> GetAllVehicles()
        {
            var response = await _vehicleService.GetAllVehicles();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var cType = ((ApiOkResponse)response).Result;
            return Ok(cType);
        }

        [HttpGet("GetVehicleById/{id}")]
        public async Task<ActionResult> GetVehicleById(long id)
        {
            var response = await _vehicleService.GetVehicleById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var type = ((ApiOkResponse)response).Result;
            return Ok(type);
        }

        [HttpPost("AddNewVehicle")]
        public async Task<ActionResult> AddNewVehicle(VehicleReceivingDTO ReceivingDTO)
        {
            var response = await _vehicleService.AddVehicle(HttpContext, ReceivingDTO);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var type = ((ApiOkResponse)response).Result;
            return Ok(type);
        }

        [HttpPut("UpdateVehicleById/{id}")]
        public async Task<IActionResult> UpdateTypeById(long id, VehicleReceivingDTO Receiving)
        {
            var response = await _vehicleService.UpdateVehicle(HttpContext, id, Receiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var type = ((ApiOkResponse)response).Result;
            return Ok(type);
        }

        [HttpDelete("DeleteVehicleById/{id}")] //{id}
        public async Task<ActionResult> DeleteVehicleById(int id)
        {
            var response = await _vehicleService.DeleteVehicle(id);
            return StatusCode(response.StatusCode);
        }

    }
}
