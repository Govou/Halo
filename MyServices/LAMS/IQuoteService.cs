using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices.LAMS
{
    public interface IQuoteService
    {
        Task<ApiResponse> AddQuote(HttpContext context, QuoteReceivingDTO quoteReceivingDTO);
        Task<ApiResponse> GetAllQuote();
        Task<ApiResponse> GetQuoteById(long id);
        Task<ApiResponse> GetQuoteByReferenceNumber(string referenceNumber);
        Task<ApiResponse> UpdateQuote(HttpContext context, long id, QuoteReceivingDTO quoteReceivingDTO);
        Task<ApiResponse> DeleteQuote(long id);

    }
}