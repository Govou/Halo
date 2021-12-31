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
    public class LeadDivisionController : ControllerBase
    {
        private readonly ILeadDivisionService _leadDivisionService;

        public LeadDivisionController(ILeadDivisionService leadDivisionService)
        {
            this._leadDivisionService = leadDivisionService;
        }

        [HttpGet("")]
        public async Task<ActionResult> GetLeadDivision()
        {
            var response = await _leadDivisionService.GetAllLeadDivision();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var leadDivision = ((ApiOkResponse)response).Result;
            return Ok(leadDivision);
        }
        [HttpGet("getbyname/{name}")]
        public async Task<ActionResult> GetByName(string name)
        {
            var response = await _leadDivisionService.GetLeadDivisionByName(name);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var leadDivision = ((ApiOkResponse)response).Result;
            return Ok(leadDivision);
        }

        [HttpGet("getbyrcnumber/{rcNumber}")]
        public async Task<ActionResult> GetByReferenceNumber(string rcNumber)
        {
            var response = await _leadDivisionService.GetLeadDivisionByRCNumber(rcNumber);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var leadDivision = ((ApiOkResponse)response).Result;
            return Ok(leadDivision);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var response = await _leadDivisionService.GetLeadDivisionById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var leadDivision = ((ApiOkResponse)response).Result;
            return Ok(leadDivision);
        }

        [HttpPost("")]
        public async Task<ActionResult> AddNewLeadDivision(LeadDivisionReceivingDTO leadDivisionReceiving)
        {
            var response = await _leadDivisionService.AddLeadDivision(HttpContext, leadDivisionReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var leadDivision = ((ApiOkResponse)response).Result;
            return Ok(leadDivision);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, LeadDivisionReceivingDTO leadDivisionReceivingDTO)
        {
            var response = await _leadDivisionService.UpdateLeadDivision(HttpContext, id, leadDivisionReceivingDTO);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var leadDivision = ((ApiOkResponse)response).Result;
            return Ok(leadDivision);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            var response = await _leadDivisionService.DeleteLeadDivision(id);
            return StatusCode(response.StatusCode);
        }
    }
}