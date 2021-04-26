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

namespace HaloBiz.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProspectController : ControllerBase
    {
        private readonly IProspectService _ProspectService;

        public ProspectController(IProspectService prospectService)
        {
            this._ProspectService = prospectService;
        }

        [HttpGet("")]
        public async Task<ActionResult> GetProspect()
        {
            var response = await _ProspectService.GetAllProspect();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Prospect = ((ApiOkResponse)response).Result;
            return Ok(Prospect);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var response = await _ProspectService.GetProspectById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Prospect = ((ApiOkResponse)response).Result;
            return Ok(Prospect);
        }

        [HttpPost("")]
        public async Task<ActionResult> AddNewProspect(ProspectReceivingDTO ProspectReceiving)
        {
            var response = await _ProspectService.AddProspect(HttpContext, ProspectReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Prospect = ((ApiOkResponse)response).Result;
            return Ok(Prospect);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, ProspectReceivingDTO ProspectReceiving)
        {
            var response = await _ProspectService.UpdateProspect(HttpContext, id, ProspectReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Prospect = ((ApiOkResponse)response).Result;
            return Ok(Prospect);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            var response = await _ProspectService.DeleteProspect(id);
            return StatusCode(response.StatusCode);
        }
    }
}
