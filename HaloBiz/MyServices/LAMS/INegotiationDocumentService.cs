using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices.LAMS
{
    public interface INegotiationDocumentService
    {
        Task<ApiCommonResponse> AddNegotiationDocument(HttpContext context, NegotiationDocumentReceivingDTO negotiationDocumentReceivingDTO);
        Task<ApiCommonResponse> GetAllNegotiationDocument();
        Task<ApiCommonResponse> GetNegotiationDocumentById(long id);
        Task<ApiCommonResponse> GetNegotiationDocumentByCaption(string caption);
        Task<ApiCommonResponse> UpdateNegotiationDocument(HttpContext context, long id, NegotiationDocumentReceivingDTO negotiationDocumentReceivingDTO);
        Task<ApiCommonResponse> DeleteNegotiationDocument(long id);

    }
}