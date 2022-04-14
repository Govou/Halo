using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Halobiz.Common.Auths;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs.LAMS;
using HaloBiz.MyServices.LAMS;
using Microsoft.AspNetCore.Mvc;

namespace Controllers.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ModuleName(HalobizModules.LeadAdministration,30)]

    public class QuoteServiceController : ControllerBase
    {
        private readonly IQuoteServiceService _quoteServiceService;

        public QuoteServiceController(IQuoteServiceService quoteServiceService)
        {
            this._quoteServiceService = quoteServiceService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetQuoteService()
        {
            return await _quoteServiceService.GetAllQuoteService();
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _quoteServiceService.GetQuoteServiceById(id);
        }

        [HttpGet("ByTag/{tag}")]
        public async Task<ApiCommonResponse> GetByTag(string tag)
        {
            return await _quoteServiceService.GetQuoteServiceByTag(tag);
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, List<QuoteServiceReceivingDTO> quoteServices)
        {
            return await _quoteServiceService.UpdateQuoteServicesByQuoteId(HttpContext, id, quoteServices);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _quoteServiceService.DeleteQuoteService(id);
        }
    }
}