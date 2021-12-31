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
    public class VehiclesController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;

        public VehiclesController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [HttpGet("GetAllVehicleTypes")]
        public async Task<ApiCommonResponse> GetAllVehicleTypes()
        {
            var response = await _vehicleService.GetAllVehicleTypes();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var cType = ((ApiOkResponse)response).Result;
            return Ok(cType);
        }

        [HttpGet("GetTypeById/{id}")]
        public async Task<ApiCommonResponse> GetTypeById(long id)
        {
            var response = await _vehicleService.GetVehicleTypeById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var type = ((ApiOkResponse)response).Result;
            return Ok(type);
        }

        [HttpPost("AddNewType")]
        public async Task<ApiCommonResponse> AddNewType(VehicleTypeReceivingDTO TypeReceivingDTO)
        {
            var response = await _vehicleService.AddVehicleType(HttpContext, TypeReceivingDTO);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var type = ((ApiOkResponse)response).Result;
            return Ok(type);
        }

        [HttpPut("UpdateTypeById/{id}")]
        public async Task<IActionResult> UpdateTypeById(long id, VehicleTypeReceivingDTO TypeReceiving)
        {
            var response = await _vehicleService.UpdateVehicleType(HttpContext, id, TypeReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var type = ((ApiOkResponse)response).Result;
            return Ok(type);
        }

        [HttpDelete("DeleteTypeById/{id}")] //{id}
        public async Task<ApiCommonResponse> DeleteTypeById(int id)
        {
            var response = await _vehicleService.DeleteVehicleType(id);
            return StatusCode(response.StatusCode);
        }

    }
}
