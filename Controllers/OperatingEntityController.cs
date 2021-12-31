using HaloBiz.DTOs.ApiDTOs;
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
    public class OperatingEntityController : ControllerBase
    {
        private readonly IOperatingEntityService _operatingEntityService;

        public OperatingEntityController(IOperatingEntityService operatingEntityService)
        {
            this._operatingEntityService = operatingEntityService;
        }

        [HttpGet("")]
        public async Task<ActionResult> GetOperatingEntities()
        {
            var response = await _operatingEntityService.GetAllOperatingEntities();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var operatingEntity = ((ApiOkResponse)response).Result;
            return Ok((IEnumerable<OperatingEntityTransferDTO>)operatingEntity);
        }
        [HttpGet("SbuProportion")]
        public async Task<ActionResult> GetOperatingEntitiesAndSbuproportion()
        {
            var response = await _operatingEntityService.GetAllOperatingEntitiesAndSbuproportion();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var operatingEntity = ((ApiOkResponse)response).Result;
            return Ok(operatingEntity);
        }
        [HttpGet("name/{name}")]
        public async Task<ActionResult> GetByName(string name)
        {
            var response = await _operatingEntityService.GetOperatingEntityByName(name);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var operatingEntity = ((ApiOkResponse)response).Result;
            return Ok((OperatingEntityTransferDTO)operatingEntity);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var response = await _operatingEntityService.GetOperatingEntityById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var operatingEntity = ((ApiOkResponse)response).Result;
            return Ok((OperatingEntityTransferDTO)operatingEntity);
        }

        [HttpPost("")]
        public async Task<ActionResult> AddNew(OperatingEntityReceivingDTO operatingEntityReceiving)
        {
            var response = await _operatingEntityService.AddOperatingEntity(operatingEntityReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var operatingEntity = ((ApiOkResponse)response).Result;
            return Ok((OperatingEntityTransferDTO)operatingEntity);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, OperatingEntityReceivingDTO operatingEntityReceiving)
        {
            var response = await _operatingEntityService.UpdateOperatingEntity(id, operatingEntityReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var operatingEntity = ((ApiOkResponse)response).Result;
            return Ok((OperatingEntityTransferDTO)operatingEntity);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            var response = await _operatingEntityService.DeleteOperatingEntity(id);
            return StatusCode(response.StatusCode);
        }
    }
}
