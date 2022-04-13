using Halobiz.Common.Auths;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
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
    [ModuleName(HalobizModules.Setups)]

    public class StrategicBusinessUnitController : ControllerBase
    {
        private readonly IStrategicBusinessUnitService _strategicBusinessUnitService;

        public StrategicBusinessUnitController(IStrategicBusinessUnitService strategicBusinessUnitService)
        {
            this._strategicBusinessUnitService = strategicBusinessUnitService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetStrategicBusinessUnits()
        {
            return await _strategicBusinessUnitService.GetAllStrategicBusinessUnit(); 
        }

        [HttpGet("GetRMSbus")]
        public async Task<ApiCommonResponse> GetRMSbus()
        {
            return await _strategicBusinessUnitService.GetRMSbus(); 
        }

        [HttpGet("GetRMSbusWithClientsInfo")]
        public async Task<ApiCommonResponse> GetRMSbusWithClientsInfo()
        {
            return await _strategicBusinessUnitService.GetRMSbusWithClientsInfo(); 
        }


        [HttpGet("name/{name}")]
        public async Task<ApiCommonResponse> GetByName(string name)
        {
            return await _strategicBusinessUnitService.GetStrategicBusinessUnitByName(name); 
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _strategicBusinessUnitService.GetStrategicBusinessUnitById(id); 
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNew(StrategicBusinessUnitReceivingDTO strategicBusinessUnitReceiving)
        {
            return await _strategicBusinessUnitService.AddStrategicBusinessUnit(strategicBusinessUnitReceiving); 
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, StrategicBusinessUnitReceivingDTO strategicBusinessUnitReceiving)
        {
            return await _strategicBusinessUnitService.UpdateStrategicBusinessUnit(id, strategicBusinessUnitReceiving); 
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _strategicBusinessUnitService.DeleteStrategicBusinessUnit(id);
        }
    }
}
