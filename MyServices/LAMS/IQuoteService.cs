using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices.LAMS
{
    public interface IQuoteService
    {
        Task<ApiCommonResponse> AddQuote(HttpContext context, QuoteReceivingDTO quoteReceivingDTO);
        Task<ApiCommonResponse> GetAllQuote();
        Task<ApiCommonResponse> GetQuoteById(long id);
        Task<ApiCommonResponse> FindByLeadDivisionId(long id);
        Task<ApiCommonResponse> GetQuoteByReferenceNumber(string referenceNumber);
        Task<ApiCommonResponse> UpdateQuote(HttpContext context, long id, QuoteReceivingDTO quoteReceivingDTO);
        Task<ApiCommonResponse> DeleteQuote(long id);

    }
}