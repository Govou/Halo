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
    public class DesignationController : ControllerBase
    {
        private readonly IDesignationService _designationService;

        public DesignationController(IDesignationService designationService)
        {
            this._designationService = designationService;
        }

        [HttpGet("")]
        public async Task<ActionResult> GetDesignation()
        {
            var response = await _designationService.GetAllDesignation();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var designation = ((ApiOkResponse)response).Result;
            return Ok(designation);
        }

        [HttpPost("")]
        public async Task<ActionResult> AddNewDesignation(DesignationReceivingDTO designationReceiving)
        {
            var response = await _designationService.AddDesignation(HttpContext, designationReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var designation = ((ApiOkResponse)response).Result;
            return Ok(designation);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, DesignationReceivingDTO designationReceiving)
        {
            var response = await _designationService.UpdateDesignation(HttpContext, id, designationReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var designation = ((ApiOkResponse)response).Result;
            return Ok(designation);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            var response = await _designationService.DeleteDesignation(id);
            return StatusCode(response.StatusCode);
        }

    }
}