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
    public class QuoteServiceController : ControllerBase
    {
        private readonly IQuoteServiceService _quoteServiceService;

        public QuoteServiceController(IQuoteServiceService quoteServiceService)
        {
            this._quoteServiceService = quoteServiceService;
        }

        [HttpGet("")]
        public async Task<ActionResult> GetQuoteService()
        {
            var response = await _quoteServiceService.GetAllQuoteService();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var quoteService = ((ApiOkResponse)response).Result;
            return Ok(quoteService);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var response = await _quoteServiceService.GetQuoteServiceById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var quoteService = ((ApiOkResponse)response).Result;
            return Ok(quoteService);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, QuoteServiceReceivingDTO quoteServiceReceivingDTO)
        {
            var response = await _quoteServiceService.UpdateQuoteService(HttpContext, id, quoteServiceReceivingDTO);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var quoteService = ((ApiOkResponse)response).Result;
            return Ok(quoteService);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            var response = await _quoteServiceService.DeleteQuoteService(id);
            return StatusCode(response.StatusCode);
        }
    }
}