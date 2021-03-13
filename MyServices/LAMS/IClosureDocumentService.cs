using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices.LAMS
{
    public interface IClosureDocumentService
    {
        Task<ApiResponse> AddClosureDocument(HttpContext context, ClosureDocumentReceivingDTO closureDocumentReceivingDTO);
        Task<ApiResponse> GetAllClosureDocument();
        Task<ApiResponse> GetClosureDocumentById(long id);
        Task<ApiResponse> GetClosureDocumentByCaption(string caption);
        Task<ApiResponse> UpdateClosureDocument(HttpContext context, long id, ClosureDocumentReceivingDTO closureDocumentReceivingDTO);
        Task<ApiResponse> DeleteClosureDocument(long id);

    }
}