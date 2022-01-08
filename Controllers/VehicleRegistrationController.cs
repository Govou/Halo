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
        public async Task<ApiCommonResponse> GetAllVehicles()
        {
            return await _vehicleService.GetAllVehicles(); 
        }

        [HttpGet("GetAllVehicleTies")]
        public async Task<ApiCommonResponse> GetAllVehicleTies()
        {
            return await _vehicleService.GetAllVehicleTies(); 
        }

        [HttpGet("GetAllVehicleTiesByResourceId")]
        public async Task<ActionResult> GetAllVehicleTiesByResourceId(long id)
        {
            var response = await _vehicleService.GetAllVehicleTiesByResourceId(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var cType = ((ApiOkResponse)response).Result;
            return Ok(cType);
        }

        [HttpGet("GetVehicleById/{id}")]
        public async Task<ApiCommonResponse> GetVehicleById(long id)
        {
            return await _vehicleService.GetVehicleById(id); 
        }

        [HttpGet("GetVehicleTieById/{id}")]
        public async Task<ApiCommonResponse> GetVehicleTieById(long id)
        {
            return await _vehicleService.GetVehicleTieById(id); 
        }

        [HttpPost("AddNewVehicle")]
        public async Task<ApiCommonResponse> AddNewVehicle(VehicleReceivingDTO ReceivingDTO)
        {
            return await _vehicleService.AddVehicle(HttpContext, ReceivingDTO); 
        }

        [HttpPost("AddNewVehicleTie")]
        public async Task<ApiCommonResponse> AddNewVehicleTie(VehicleSMORoutesResourceTieReceivingDTO ReceivingDTO)
        {
            return await _vehicleService.AddVehicleTie(HttpContext, ReceivingDTO); 
        }

        [HttpPut("UpdateVehicleById/{id}")]
        public async Task<ApiCommonResponse> UpdateTypeById(long id, VehicleReceivingDTO Receiving)
        {
            return await _vehicleService.UpdateVehicle(HttpContext, id, Receiving); 
        }

        [HttpDelete("DeleteVehicleById/{id}")] //{id}
        public async Task<ApiCommonResponse> DeleteVehicleById(int id)
        {
            return await _vehicleService.DeleteVehicle(id);
        }
        [HttpDelete("DeleteVehicleTieById/{id}")] //{id}
        public async Task<ApiCommonResponse> DeleteVehicleTieById(int id)
        {
            return await _vehicleService.DeleteVehicleTie(id);
         }

    }
}
