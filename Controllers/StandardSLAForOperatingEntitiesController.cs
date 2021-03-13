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
    public class StandardSLAForOperatingEntitiesController : ControllerBase
    {
        private readonly IStandardSLAForOperatingEntitiesService _standardSLAForOperatingEntitiesService;

        public StandardSLAForOperatingEntitiesController(IStandardSLAForOperatingEntitiesService standardSLAForOperatingEntitiesService)
        {
            this._standardSLAForOperatingEntitiesService = standardSLAForOperatingEntitiesService;
        }

        [HttpGet("")]
        public async Task<ActionResult> GetStandardSLAForOperatingEntities()
        {
            var response = await _standardSLAForOperatingEntitiesService.GetAllStandardSLAForOperatingEntities();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var standardSLAForOperatingEntities = ((ApiOkResponse)response).Result;
            return Ok(standardSLAForOperatingEntities);
        }
        [HttpGet("caption/{name}")]
        public async Task<ActionResult> GetByCaption(string name)
        {
            var response = await _standardSLAForOperatingEntitiesService.GetStandardSLAForOperatingEntitiesByName(name);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var standardSLAForOperatingEntities = ((ApiOkResponse)response).Result;
            return Ok(standardSLAForOperatingEntities);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var response = await _standardSLAForOperatingEntitiesService.GetStandardSLAForOperatingEntitiesById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var standardSLAForOperatingEntities = ((ApiOkResponse)response).Result;
            return Ok(standardSLAForOperatingEntities);
        }

        [HttpPost("")]
        public async Task<ActionResult> AddNewStandardSLAForOperatingEntities(StandardSLAForOperatingEntitiesReceivingDTO standardSLAForOperatingEntitiesReceiving)
        {
            var response = await _standardSLAForOperatingEntitiesService.AddStandardSLAForOperatingEntities(HttpContext, standardSLAForOperatingEntitiesReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var standardSLAForOperatingEntities = ((ApiOkResponse)response).Result;
            return Ok(standardSLAForOperatingEntities);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, StandardSLAForOperatingEntitiesReceivingDTO standardSLAForOperatingEntitiesReceiving)
        {
            var response = await _standardSLAForOperatingEntitiesService.UpdateStandardSLAForOperatingEntities(HttpContext, id, standardSLAForOperatingEntitiesReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var standardSLAForOperatingEntities = ((ApiOkResponse)response).Result;
            return Ok(standardSLAForOperatingEntities);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            var response = await _standardSLAForOperatingEntitiesService.DeleteStandardSLAForOperatingEntities(id);
            return StatusCode(response.StatusCode);
        }
    }
}