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
    public class SuspectController : ControllerBase
    {
        private readonly ISuspectService _SuspectService;

        public SuspectController(ISuspectService suspectService)
        {
            this._SuspectService = suspectService;
        }

        [HttpGet("")]
        public async Task<ActionResult> GetSuspect()
        {
            var response = await _SuspectService.GetAllSuspect();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Suspect = ((ApiOkResponse)response).Result;
            return Ok(Suspect);
        }

        /*[HttpGet("caption/{name}")]
        public async Task<ActionResult> GetByCaption(string name)
        {
            var response = await _SuspectService.GetSuspectByName(name);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Suspect = ((ApiOkResponse)response).Result;
            return Ok(Suspect);
        }*/

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var response = await _SuspectService.GetSuspectById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Suspect = ((ApiOkResponse)response).Result;
            return Ok(Suspect);
        }

        [HttpPost("")]
        public async Task<ActionResult> AddNewSuspect(SuspectReceivingDTO SuspectReceiving)
        {
            var response = await _SuspectService.AddSuspect(HttpContext, SuspectReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Suspect = ((ApiOkResponse)response).Result;
            return Ok(Suspect);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, SuspectReceivingDTO SuspectReceiving)
        {
            var response = await _SuspectService.UpdateSuspect(HttpContext, id, SuspectReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Suspect = ((ApiOkResponse)response).Result;
            return Ok(Suspect);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            var response = await _SuspectService.DeleteSuspect(id);
            return StatusCode(response.StatusCode);
        }
    }
}
