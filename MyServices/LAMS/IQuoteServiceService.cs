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
        Task<ApiResponse> UpdateQuoteService(HttpContext context, long id, QuoteServiceReceivingDTO quoteServiceReceivingDTO);
        Task<ApiResponse> DeleteQuoteService(long id);

    }
}