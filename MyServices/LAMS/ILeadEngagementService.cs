using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices.LAMS
{
    public interface ILeadEngagementService
    {
        Task<ApiCommonResponse> AddLeadEngagement(HttpContext context, LeadEngagementReceivingDTO leadEngagementReceivingDTO);
        Task<ApiCommonResponse> GetAllLeadEngagement();
        Task<ApiCommonResponse> GetLeadEngagementById(long id);
        Task<ApiCommonResponse> FindLeadEngagementsByLeadId(long leadId);
        Task<ApiCommonResponse> GetLeadEngagementByName(string name);
        Task<ApiCommonResponse> UpdateLeadEngagement(HttpContext context, long id, LeadEngagementReceivingDTO leadEngagementReceivingDTO);
        Task<ApiCommonResponse> DeleteLeadEngagement(long id);

    }
}