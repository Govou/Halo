using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices.LAMS
{
    public interface IClientEngagementService
    {
        Task<ApiCommonResponse> AddClientEngagement(HttpContext context, ClientEngagementReceivingDTO clientEngagementReceivingDTO);
        Task<ApiCommonResponse> GetAllClientEngagement();
        Task<ApiCommonResponse> GetClientEngagementById(long id);
        Task<ApiCommonResponse> GetClientEngagementByName(string name);
        Task<ApiCommonResponse> UpdateClientEngagement(HttpContext context, long id, ClientEngagementReceivingDTO clientEngagementReceivingDTO);
        Task<ApiCommonResponse> DeleteClientEngagement(long id);

    }
}