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
    public class QuoteController : ControllerBase
    {
        private readonly IQuoteService _quoteService;

        public QuoteController(IQuoteService quoteService)
        {
            this._quoteService = quoteService;
        }

        [HttpGet("")]
        public async Task<ActionResult> GetQuote()
        {
            var response = await _quoteService.GetAllQuote();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var quote = ((ApiOkResponse)response).Result;
            return Ok(quote);
        }
        [HttpGet("reference/{reference}")]
        public async Task<ActionResult> GetByReferenceNumber(string reference)
        {
            var response = await _quoteService.GetQuoteByReferenceNumber(reference);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var quote = ((ApiOkResponse)response).Result;
            return Ok(quote);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var response = await _quoteService.GetQuoteById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var quote = ((ApiOkResponse)response).Result;
            return Ok(quote);
        }

        [HttpPost("")]
        public async Task<ActionResult> AddNewQuote(QuoteReceivingDTO quoteReceiving)
        {
            var response = await _quoteService.AddQuote(HttpContext, quoteReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var quote = ((ApiOkResponse)response).Result;
            return Ok(quote);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, QuoteReceivingDTO quoteReceivingDTO)
        {
            var response = await _quoteService.UpdateQuote(HttpContext, id, quoteReceivingDTO);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var quote = ((ApiOkResponse)response).Result;
            return Ok(quote);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            var response = await _quoteService.DeleteQuote(id);
            return StatusCode(response.StatusCode);
        }
    }
}