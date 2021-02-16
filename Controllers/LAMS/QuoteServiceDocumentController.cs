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
    public class QuoteServiceDocumentController : ControllerBase
    {
        private readonly IQuoteServiceDocumentService _quoteServiceDocumentService;

        public QuoteServiceDocumentController(IQuoteServiceDocumentService quoteServiceDocumentService)
        {
            this._quoteServiceDocumentService = quoteServiceDocumentService;
        }

        [HttpGet("")]
        public async Task<ActionResult> GetQuoteServiceDocument()
        {
            var response = await _quoteServiceDocumentService.GetAllQuoteServiceDocument();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var quoteServiceDocument = ((ApiOkResponse)response).Result;
            return Ok(quoteServiceDocument);
        }

        [HttpGet("QuoteService/{quoteServiceId}")]
        public async Task<ActionResult> GetQuoteServiceDocumentsForAQuoteService(long quoteServiceId)
        {
            var response = await _quoteServiceDocumentService.GetAllQuoteServiceDocumentForAQuoteService(quoteServiceId);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var quoteServiceDocument = ((ApiOkResponse)response).Result;
            return Ok(quoteServiceDocument);
        }

        [HttpGet("caption/{caption}")]
        public async Task<ActionResult> GetByCaption(string caption)
        {
            var response = await _quoteServiceDocumentService.GetQuoteServiceDocumentByCaption(caption);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var quoteServiceDocument = ((ApiOkResponse)response).Result;
            return Ok(quoteServiceDocument);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var response = await _quoteServiceDocumentService.GetQuoteServiceDocumentById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var quoteServiceDocument = ((ApiOkResponse)response).Result;
            return Ok(quoteServiceDocument);
        }

        [HttpPost("")]
        public async Task<ActionResult> AddNewQuoteServiceDocument(QuoteServiceDocumentReceivingDTO quoteServiceDocumentReceiving)
        {
            var response = await _quoteServiceDocumentService.AddQuoteServiceDocument(HttpContext, quoteServiceDocumentReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var quoteServiceDocument = ((ApiOkResponse)response).Result;
            return Ok(quoteServiceDocument);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, QuoteServiceDocumentReceivingDTO quoteServiceDocumentReceivingDTO)
        {
            var response = await _quoteServiceDocumentService.UpdateQuoteServiceDocument(HttpContext, id, quoteServiceDocumentReceivingDTO);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var quoteServiceDocument = ((ApiOkResponse)response).Result;
            return Ok(quoteServiceDocument);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            var response = await _quoteServiceDocumentService.DeleteQuoteServiceDocument(id);
            return StatusCode(response.StatusCode);
        }
    }
}