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
    public class ClosureDocumentController : ControllerBase
    {
        private readonly IClosureDocumentService _closureDocumentService;

        public ClosureDocumentController(IClosureDocumentService closureDocumentService)
        {
            this._closureDocumentService = closureDocumentService;
        }

        [HttpGet("")]
        public async Task<ActionResult> GetClosureDocument()
        {
            var response = await _closureDocumentService.GetAllClosureDocument();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var closureDocument = ((ApiOkResponse)response).Result;
            return Ok(closureDocument);
        }
        [HttpGet("caption/{caption}")]
        public async Task<ActionResult> GetByCaption(string caption)
        {
            var response = await _closureDocumentService.GetClosureDocumentByCaption(caption);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var closureDocument = ((ApiOkResponse)response).Result;
            return Ok(closureDocument);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var response = await _closureDocumentService.GetClosureDocumentById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var closureDocument = ((ApiOkResponse)response).Result;
            return Ok(closureDocument);
        }

        [HttpPost("")]
        public async Task<ActionResult> AddNewClosureDocument(ClosureDocumentReceivingDTO closureDocumentReceiving)
        {
            var response = await _closureDocumentService.AddClosureDocument(HttpContext, closureDocumentReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var closureDocument = ((ApiOkResponse)response).Result;
            return Ok(closureDocument);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, ClosureDocumentReceivingDTO closureDocumentReceivingDTO)
        {
            var response = await _closureDocumentService.UpdateClosureDocument(HttpContext, id, closureDocumentReceivingDTO);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var closureDocument = ((ApiOkResponse)response).Result;
            return Ok(closureDocument);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            var response = await _closureDocumentService.DeleteClosureDocument(id);
            return StatusCode(response.StatusCode);
        }
    }
}