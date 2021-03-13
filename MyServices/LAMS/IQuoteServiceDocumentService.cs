using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices.LAMS
{
    public interface IQuoteServiceDocumentService
    {
        Task<ApiResponse> AddQuoteServiceDocument(HttpContext context, QuoteServiceDocumentReceivingDTO quoteServiceDocumentReceivingDTO);
        Task<ApiResponse> GetAllQuoteServiceDocument();
        Task<ApiResponse> GetQuoteServiceDocumentById(long id);
        Task<ApiResponse> GetQuoteServiceDocumentByCaption(string caption);
        Task<ApiResponse> UpdateQuoteServiceDocument(HttpContext context, long id, QuoteServiceDocumentReceivingDTO quoteServiceDocumentReceivingDTO);
        Task<ApiResponse> DeleteQuoteServiceDocument(long id);
        Task<ApiResponse> GetAllQuoteServiceDocumentForAQuoteService(long quoteServiceId);

    }
}