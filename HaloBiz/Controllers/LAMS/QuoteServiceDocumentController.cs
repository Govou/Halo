using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Halobiz.Common.DTOs.ApiDTOs;
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
        public async Task<ApiCommonResponse> GetQuoteServiceDocument()
        {
            return await _quoteServiceDocumentService.GetAllQuoteServiceDocument();
        }

        [HttpGet("QuoteService/{quoteServiceId}")]
        public async Task<ApiCommonResponse> GetQuoteServiceDocumentsForAQuoteService(long quoteServiceId)
        {
            return await _quoteServiceDocumentService.GetAllQuoteServiceDocumentForAQuoteService(quoteServiceId);
        }

        [HttpGet("caption/{caption}")]
        public async Task<ApiCommonResponse> GetByCaption(string caption)
        {
            return await _quoteServiceDocumentService.GetQuoteServiceDocumentByCaption(caption);
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _quoteServiceDocumentService.GetQuoteServiceDocumentById(id);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewQuoteServiceDocument(QuoteServiceDocumentReceivingDTO quoteServiceDocumentReceiving)
        {
            return await _quoteServiceDocumentService.AddQuoteServiceDocument(HttpContext, quoteServiceDocumentReceiving);
            
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, QuoteServiceDocumentReceivingDTO quoteServiceDocumentReceivingDTO)
        {
            return await _quoteServiceDocumentService.UpdateQuoteServiceDocument(HttpContext, id, quoteServiceDocumentReceivingDTO);            
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _quoteServiceDocumentService.DeleteQuoteServiceDocument(id);
        }
    }
}