using System.Threading.Tasks;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices
{
    public interface IRequiredServiceDocumentService
    {
        Task<ApiCommonResponse> AddRequiredServiceDocument(HttpContext context, RequiredServiceDocumentReceivingDTO requiredServiceDocumentReceivingDTO);
        Task<ApiCommonResponse> GetAllRequiredServiceDocument();
        Task<ApiCommonResponse> GetRequiredServiceDocumentById(long id);
        Task<ApiCommonResponse> DeleteRequiredServiceDocument(long id);
        Task<ApiCommonResponse> UpdateRequiredServiceDocument(HttpContext context, long id, RequiredServiceDocumentReceivingDTO requiredServiceDocumentReceivingDTO);
        Task<ApiCommonResponse> GetRequiredServiceDocumentByName(string name);
    }
}