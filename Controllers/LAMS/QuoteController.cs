using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs.LAMS;
using HaloBiz.MyServices.LAMS;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ApiCommonResponse> GetQuote()
        {
            return await _quoteService.GetAllQuote();
        }

        [HttpGet("reference/{reference}")]
        public async Task<ApiCommonResponse> GetByReferenceNumber(string reference)
        {
            return await _quoteService.GetQuoteByReferenceNumber(reference);
        }

        [HttpGet("ByLeadDivision/{id}")]
        public async Task<ApiCommonResponse> ByLeadDivision(long id)
        {
            return await _quoteService.FindByLeadDivisionId(id);
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _quoteService.GetQuoteById(id);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewQuote(QuoteReceivingDTO quoteReceiving)
        {
            return await _quoteService.AddQuote(HttpContext, quoteReceiving);
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, QuoteReceivingDTO quoteReceivingDTO)
        {
            return await _quoteService.UpdateQuote(HttpContext, id, quoteReceivingDTO);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _quoteService.DeleteQuote(id);
        }
    }
}