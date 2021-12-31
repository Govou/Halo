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
    public class NegotiationDocumentController : ControllerBase
    {
        private readonly INegotiationDocumentService _negotiationDocumentService;

        public NegotiationDocumentController(INegotiationDocumentService negotiationDocumentService)
        {
            this._negotiationDocumentService = negotiationDocumentService;
        }

        [HttpGet("")]
        public async Task<ActionResult> GetNegotiationDocument()
        {
            var response = await _negotiationDocumentService.GetAllNegotiationDocument();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var negotiationDocument = ((ApiOkResponse)response).Result;
            return Ok(negotiationDocument);
        }
        [HttpGet("caption/{caption}")]
        public async Task<ActionResult> GetByCaption(string caption)
        {
            var response = await _negotiationDocumentService.GetNegotiationDocumentByCaption(caption);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var negotiationDocument = ((ApiOkResponse)response).Result;
            return Ok(negotiationDocument);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var response = await _negotiationDocumentService.GetNegotiationDocumentById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var negotiationDocument = ((ApiOkResponse)response).Result;
            return Ok(negotiationDocument);
        }

        [HttpPost("")]
        public async Task<ActionResult> AddNewNegotiationDocument(NegotiationDocumentReceivingDTO negotiationDocumentReceiving)
        {
            var response = await _negotiationDocumentService.AddNegotiationDocument(HttpContext, negotiationDocumentReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var negotiationDocument = ((ApiOkResponse)response).Result;
            return Ok(negotiationDocument);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, NegotiationDocumentReceivingDTO negotiationDocumentReceivingDTO)
        {
            var response = await _negotiationDocumentService.UpdateNegotiationDocument(HttpContext, id, negotiationDocumentReceivingDTO);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var negotiationDocument = ((ApiOkResponse)response).Result;
            return Ok(negotiationDocument);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            var response = await _negotiationDocumentService.DeleteNegotiationDocument(id);
            return StatusCode(response.StatusCode);
        }
    }
}