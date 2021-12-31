using System.Collections.Generic;
using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices.LAMS
{
    public interface IQuoteServiceService
    {
        Task<ApiResponse> AddQuoteService(HttpContext context, QuoteServiceReceivingDTO quoteServiceReceivingDTO);
        Task<ApiResponse> GetAllQuoteService();
        Task<ApiResponse> GetQuoteServiceById(long id);
        Task<ApiResponse> UpdateQuoteServicesByQuoteId(HttpContext context, long quoteId, IEnumerable<QuoteServiceReceivingDTO> quoteServices);
        Task<ApiResponse> GetQuoteServiceByTag(string tag);
        Task<ApiResponse> UpdateQuoteService(HttpContext context, long id, QuoteServiceReceivingDTO quoteServiceReceivingDTO);
        Task<ApiResponse> DeleteQuoteService(long id);

    }
}