using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices.LAMS
{
    public interface IQuoteServiceDocumentService
    {
        Task<ApiCommonResponse> AddQuoteServiceDocument(HttpContext context, QuoteServiceDocumentReceivingDTO quoteServiceDocumentReceivingDTO);
        Task<ApiCommonResponse> GetAllQuoteServiceDocument();
        Task<ApiCommonResponse> GetQuoteServiceDocumentById(long id);
        Task<ApiCommonResponse> GetQuoteServiceDocumentByCaption(string caption);
        Task<ApiCommonResponse> UpdateQuoteServiceDocument(HttpContext context, long id, QuoteServiceDocumentReceivingDTO quoteServiceDocumentReceivingDTO);
        Task<ApiCommonResponse> DeleteQuoteServiceDocument(long id);
        Task<ApiCommonResponse> GetAllQuoteServiceDocumentForAQuoteService(long quoteServiceId);

    }
}