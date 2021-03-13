using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices.LAMS
{
    public interface INegotiationDocumentService
    {
        Task<ApiResponse> AddNegotiationDocument(HttpContext context, NegotiationDocumentReceivingDTO negotiationDocumentReceivingDTO);
        Task<ApiResponse> GetAllNegotiationDocument();
        Task<ApiResponse> GetNegotiationDocumentById(long id);
        Task<ApiResponse> GetNegotiationDocumentByCaption(string caption);
        Task<ApiResponse> UpdateNegotiationDocument(HttpContext context, long id, NegotiationDocumentReceivingDTO negotiationDocumentReceivingDTO);
        Task<ApiResponse> DeleteNegotiationDocument(long id);

    }
}