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
    public class TargetController : ControllerBase
    {
        private readonly ITargetService _targetService;

        public TargetController(ITargetService targetService)
        {
            this._targetService = targetService;
        }

        [HttpGet("")]
        public async Task<ActionResult> GetTarget()
        {
            var response = await _targetService.GetAllTarget();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var target = ((ApiOkResponse)response).Result;
            return Ok(target);
        }
        [HttpGet("caption/{name}")]
        public async Task<ActionResult> GetByCaption(string name)
        {
            var response = await _targetService.GetTargetByName(name);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var target = ((ApiOkResponse)response).Result;
            return Ok(target);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var response = await _targetService.GetTargetById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var target = ((ApiOkResponse)response).Result;
            return Ok(target);
        }

        [HttpPost("")]
        public async Task<ActionResult> AddNewtarget(TargetReceivingDTO targetReceiving)
        {
            var response = await _targetService.AddTarget(HttpContext, targetReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var target = ((ApiOkResponse)response).Result;
            return Ok(target);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, TargetReceivingDTO targetReceiving)
        {
            var response = await _targetService.UpdateTarget(HttpContext, id, targetReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var target = ((ApiOkResponse)response).Result;
            return Ok(target);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            var response = await _targetService.DeleteTarget(id);
            return StatusCode(response.StatusCode);
        }
    }
}