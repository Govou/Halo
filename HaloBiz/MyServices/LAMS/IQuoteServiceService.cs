using System.Collections.Generic;
using System.Threading.Tasks;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices.LAMS
{
    public interface IQuoteServiceService
    {
        Task<ApiCommonResponse> AddQuoteService(HttpContext context, QuoteServiceReceivingDTO quoteServiceReceivingDTO);
        Task<ApiCommonResponse> GetAllQuoteService();
        Task<ApiCommonResponse> GetQuoteServiceById(long id);
        Task<ApiCommonResponse> UpdateQuoteServicesByQuoteId(HttpContext context, long quoteId, IEnumerable<QuoteServiceReceivingDTO> quoteServices);
        Task<ApiCommonResponse> GetQuoteServiceByTag(string tag);
        Task<ApiCommonResponse> UpdateQuoteService(HttpContext context, long id, QuoteServiceReceivingDTO quoteServiceReceivingDTO);
        Task<ApiCommonResponse> DeleteQuoteService(long id);

    }
}