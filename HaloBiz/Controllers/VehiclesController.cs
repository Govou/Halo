using Halobiz.Common.Auths;
using Halobiz.Common.DTOs.ApiDTOs;
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
    [ModuleName(HalobizModules.SecuredMobility,115)]
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
            return await _vehicleService.GetAllVehicleTypes(); 
        }

        [HttpGet("GetTypeById/{id}")]
        public async Task<ApiCommonResponse> GetTypeById(long id)
        {
            return await _vehicleService.GetVehicleTypeById(id); 
        }

        [HttpPost("AddNewType")]
        public async Task<ApiCommonResponse> AddNewType(VehicleTypeReceivingDTO TypeReceivingDTO)
        {
            return await _vehicleService.AddVehicleType(HttpContext, TypeReceivingDTO); 
        }

        [HttpPut("UpdateTypeById/{id}")]
        public async Task<ApiCommonResponse> UpdateTypeById(long id, VehicleTypeReceivingDTO TypeReceiving)
        {
            return await _vehicleService.UpdateVehicleType(HttpContext, id, TypeReceiving); 
        }

        [HttpDelete("DeleteTypeById/{id}")] //{id}
        public async Task<ApiCommonResponse> DeleteTypeById(int id)
        {
            return await _vehicleService.DeleteVehicleType(id);
        }

    }
}
