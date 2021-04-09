using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices
{
    public interface IEngagementTypeService
    {
        Task<ApiResponse> AddEngagementType(HttpContext context, EngagementTypeReceivingDTO engagementTypeReceivingDTO);
        Task<ApiResponse> GetAllEngagementType();
        Task<ApiResponse> UpdateEngagementType(HttpContext context, long id, EngagementTypeReceivingDTO engagementTypeReceivingDTO);
        Task<ApiResponse> DeleteEngagementType(long id);
    }
}