using Halobiz.Common.Auths;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.MyServices;
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

    public class OperatingEntityController : ControllerBase
    {
        private readonly IOperatingEntityService _operatingEntityService;

        public OperatingEntityController(IOperatingEntityService operatingEntityService)
        {
            this._operatingEntityService = operatingEntityService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetOperatingEntities()
        {
            return await _operatingEntityService.GetAllOperatingEntities();
        }
        [HttpGet("SbuProportion")]
        public async Task<ApiCommonResponse> GetOperatingEntitiesAndSbuproportion()
        {
            return await _operatingEntityService.GetAllOperatingEntitiesAndSbuproportion();
        }
        [HttpGet("name/{name}")]
        public async Task<ApiCommonResponse> GetByName(string name)
        {
            return await _operatingEntityService.GetOperatingEntityByName(name);
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _operatingEntityService.GetOperatingEntityById(id);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNew(OperatingEntityReceivingDTO operatingEntityReceiving)
        {
            return await _operatingEntityService.AddOperatingEntity(operatingEntityReceiving);
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, OperatingEntityReceivingDTO operatingEntityReceiving)
        {
            return await _operatingEntityService.UpdateOperatingEntity(id, operatingEntityReceiving);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _operatingEntityService.DeleteOperatingEntity(id);
        }
    }
}
