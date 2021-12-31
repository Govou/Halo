using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices
{
    public interface IEngagementReasonService
    {
        Task<ApiResponse> AddEngagementReason(HttpContext context, EngagementReasonReceivingDTO engagementReasonReceivingDTO);
        Task<ApiResponse> GetAllEngagementReason();
        Task<ApiResponse> UpdateEngagementReason(HttpContext context, long id, EngagementReasonReceivingDTO engagementReasonReceivingDTO);
        Task<ApiResponse> DeleteEngagementReason(long id);
    }
}