using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices.LAMS
{
    public interface ILeadEngagementService
    {
        Task<ApiResponse> AddLeadEngagement(HttpContext context, LeadEngagementReceivingDTO leadEngagementReceivingDTO);
        Task<ApiResponse> GetAllLeadEngagement();
        Task<ApiResponse> GetLeadEngagementById(long id);
        Task<ApiResponse> GetLeadEngagementByName(string name);
        Task<ApiResponse> UpdateLeadEngagement(HttpContext context, long id, LeadEngagementReceivingDTO leadEngagementReceivingDTO);
        Task<ApiResponse> DeleteLeadEngagement(long id);

    }
}