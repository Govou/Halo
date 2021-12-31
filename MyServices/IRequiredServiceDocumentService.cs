using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices
{
    public interface IRequiredServiceDocumentService
    {
        Task<ApiResponse> AddRequiredServiceDocument(HttpContext context, RequiredServiceDocumentReceivingDTO requiredServiceDocumentReceivingDTO);
        Task<ApiResponse> GetAllRequiredServiceDocument();
        Task<ApiResponse> GetRequiredServiceDocumentById(long id);
        Task<ApiResponse> DeleteRequiredServiceDocument(long id);
        Task<ApiResponse> UpdateRequiredServiceDocument(HttpContext context, long id, RequiredServiceDocumentReceivingDTO requiredServiceDocumentReceivingDTO);
        Task<ApiResponse> GetRequiredServiceDocumentByName(string name);
    }
}