using System.Threading.Tasks;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices
{
    public interface IMakeService
    {
        Task<ApiCommonResponse> AddMake(HttpContext context, MakeReceivingDTO makeReceivingDTO);
        Task<ApiCommonResponse> GetAllMake();
        Task<ApiCommonResponse> DeleteMake(long id);
        Task<ApiCommonResponse> GetMakeById(long id);
    }
}