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
//using Controllers.Models;


namespace HaloBiz.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class StandardSlaforOperatingEntityController : ControllerBase
    {
        private readonly IStandardSlaforOperatingEntityService _standardSLAForOperatingEntitiesService;

        public StandardSlaforOperatingEntityController(IStandardSlaforOperatingEntityService standardSLAForOperatingEntitiesService)
        {
            this._standardSLAForOperatingEntitiesService = standardSLAForOperatingEntitiesService;
        }

        [HttpGet("")]
        public async Task<ActionResult> GetStandardSlaforOperatingEntity()
        {
            var response = await _standardSLAForOperatingEntitiesService.GetAllStandardSlaforOperatingEntity();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var standardSLAForOperatingEntities = ((ApiOkResponse)response).Result;
            return Ok(standardSLAForOperatingEntities);
        }
        [HttpGet("caption/{name}")]
        public async Task<ActionResult> GetByCaption(string name)
        {
            var response = await _standardSLAForOperatingEntitiesService.GetStandardSlaforOperatingEntityByName(name);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var standardSLAForOperatingEntities = ((ApiOkResponse)response).Result;
            return Ok(standardSLAForOperatingEntities);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var response = await _standardSLAForOperatingEntitiesService.GetStandardSlaforOperatingEntityById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var standardSLAForOperatingEntities = ((ApiOkResponse)response).Result;
            return Ok(standardSLAForOperatingEntities);
        }

        [HttpPost("")]
        public async Task<ActionResult> AddNewStandardSlaforOperatingEntity(StandardSlaforOperatingEntityReceivingDTO standardSLAForOperatingEntitiesReceiving)
        {
            var response = await _standardSLAForOperatingEntitiesService.AddStandardSlaforOperatingEntity(HttpContext, standardSLAForOperatingEntitiesReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var standardSLAForOperatingEntities = ((ApiOkResponse)response).Result;
            return Ok(standardSLAForOperatingEntities);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, StandardSlaforOperatingEntityReceivingDTO standardSLAForOperatingEntitiesReceiving)
        {
            var response = await _standardSLAForOperatingEntitiesService.UpdateStandardSlaforOperatingEntity(HttpContext, id, standardSLAForOperatingEntitiesReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var standardSLAForOperatingEntities = ((ApiOkResponse)response).Result;
            return Ok(standardSLAForOperatingEntities);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            var response = await _standardSLAForOperatingEntitiesService.DeleteStandardSlaforOperatingEntity(id);
            return StatusCode(response.StatusCode);
        }
    }
}