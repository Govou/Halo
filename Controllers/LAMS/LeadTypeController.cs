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
    public class LeadTypeController : ControllerBase
    {
        private readonly ILeadTypeService _leadTypeService;

        public LeadTypeController(ILeadTypeService leadTypeService)
        {
            this._leadTypeService = leadTypeService;
        }

        [HttpGet("")]
        public async Task<ActionResult> GetLeadType()
        {
            var response = await _leadTypeService.GetAllLeadType();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var leadType = ((ApiOkResponse)response).Result;
            return Ok(leadType);
        }
        [HttpGet("caption/{name}")]
        public async Task<ActionResult> GetByCaption(string name)
        {
            var response = await _leadTypeService.GetLeadTypeByName(name);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var leadType = ((ApiOkResponse)response).Result;
            return Ok(leadType);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var response = await _leadTypeService.GetLeadTypeById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var leadType = ((ApiOkResponse)response).Result;
            return Ok(leadType);
        }

        [HttpPost("")]
        public async Task<ActionResult> AddNewLeadType(LeadTypeReceivingDTO leadTypeReceiving)
        {
            var response = await _leadTypeService.AddLeadType(HttpContext, leadTypeReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var leadType = ((ApiOkResponse)response).Result;
            return Ok(leadType);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, LeadTypeReceivingDTO leadTypeReceivingDTO)
        {
            var response = await _leadTypeService.UpdateLeadType(HttpContext, id, leadTypeReceivingDTO);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var leadType = ((ApiOkResponse)response).Result;
            return Ok(leadType);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            var response = await _leadTypeService.DeleteLeadType(id);
            return StatusCode(response.StatusCode);
        }
    }
}