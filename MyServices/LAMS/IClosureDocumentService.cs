using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices.LAMS
{
    public interface IClosureDocumentService
    {
        Task<ApiCommonResponse> AddClosureDocument(HttpContext context, ClosureDocumentReceivingDTO closureDocumentReceivingDTO);
        Task<ApiCommonResponse> GetAllClosureDocument();
        Task<ApiCommonResponse> GetClosureDocumentById(long id);
        Task<ApiCommonResponse> GetClosureDocumentByCaption(string caption);
        Task<ApiCommonResponse> UpdateClosureDocument(HttpContext context, long id, ClosureDocumentReceivingDTO closureDocumentReceivingDTO);
        Task<ApiCommonResponse> DeleteClosureDocument(long id);

    }
}