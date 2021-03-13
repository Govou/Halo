using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs.LAMS;
using HaloBiz.MyServices.LAMS;
using Microsoft.AspNetCore.Mvc;
//using Controllers.Models;

namespace Controllers.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DropReasonController : ControllerBase
    {
        private readonly IDropReasonService _DropReasonService;

        public DropReasonController(IDropReasonService DropReasonService)
        {
            this._DropReasonService = DropReasonService;
        }

        [HttpGet("")]
        public async Task<ActionResult> GetDropReason()
        {
            var response = await _DropReasonService.GetAllDropReason();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var DropReason = ((ApiOkResponse)response).Result;
            return Ok(DropReason);
        }
        [HttpGet("title/{title}")]
        public async Task<ActionResult> GetByTitle(string title)
        {
            var response = await _DropReasonService.GetDropReasonByTitle(title);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var DropReason = ((ApiOkResponse)response).Result;
            return Ok(DropReason);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var response = await _DropReasonService.GetDropReasonById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var DropReason = ((ApiOkResponse)response).Result;
            return Ok(DropReason);
        }

        [HttpPost("")]
        public async Task<ActionResult> AddNewDropReason(DropReasonReceivingDTO DropReasonReceiving)
        {
            var response = await _DropReasonService.AddDropReason(HttpContext, DropReasonReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var DropReason = ((ApiOkResponse)response).Result;
            return Ok(DropReason);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, DropReasonReceivingDTO DropReasonReceiving)
        {
            var response = await _DropReasonService.UpdateDropReason(HttpContext, id, DropReasonReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var DropReason = ((ApiOkResponse)response).Result;
            return Ok(DropReason);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            var response = await _DropReasonService.DeleteDropReason(id);
            return StatusCode(response.StatusCode);
        }
    }
}