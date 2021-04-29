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
    public class EscalationMatrixController : ControllerBase
    {
        private readonly IEscalationMatrixService _EscalationMatrixService;

        public EscalationMatrixController(IEscalationMatrixService serviceTypeService)
        {
            this._EscalationMatrixService = serviceTypeService;
        }

        [HttpGet("")]
        public async Task<ActionResult> GetEscalationMatrix()
        {
            var response = await _EscalationMatrixService.GetAllEscalationMatrix();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var EscalationMatrix = ((ApiOkResponse)response).Result;
            return Ok(EscalationMatrix);
        }
        /*[HttpGet("caption/{name}")]
        public async Task<ActionResult> GetByCaption(string name)
        {
            var response = await _EscalationMatrixService.GetEscalationMatrixByName(name);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var EscalationMatrix = ((ApiOkResponse)response).Result;
            return Ok(EscalationMatrix);
        }*/

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var response = await _EscalationMatrixService.GetEscalationMatrixById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var EscalationMatrix = ((ApiOkResponse)response).Result;
            return Ok(EscalationMatrix);
        }

        [HttpPost("")]
        public async Task<ActionResult> AddNewEscalationMatrix(EscalationMatrixReceivingDTO EscalationMatrixReceiving)
        {
            var response = await _EscalationMatrixService.AddEscalationMatrix(HttpContext, EscalationMatrixReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var EscalationMatrix = ((ApiOkResponse)response).Result;
            return Ok(EscalationMatrix);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, EscalationMatrixReceivingDTO EscalationMatrixReceiving)
        {
            var response = await _EscalationMatrixService.UpdateEscalationMatrix(HttpContext, id, EscalationMatrixReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var EscalationMatrix = ((ApiOkResponse)response).Result;
            return Ok(EscalationMatrix);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            var response = await _EscalationMatrixService.DeleteEscalationMatrix(id);
            return StatusCode(response.StatusCode);
        }
    }
}
