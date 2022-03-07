using System.Threading.Tasks;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices
{
    public interface IEngagementReasonService
    {
        Task<ApiCommonResponse> AddEngagementReason(HttpContext context, EngagementReasonReceivingDTO engagementReasonReceivingDTO);
        Task<ApiCommonResponse> GetAllEngagementReason();
        Task<ApiCommonResponse> UpdateEngagementReason(HttpContext context, long id, EngagementReasonReceivingDTO engagementReasonReceivingDTO);
        Task<ApiCommonResponse> DeleteEngagementReason(long id);
    }
}