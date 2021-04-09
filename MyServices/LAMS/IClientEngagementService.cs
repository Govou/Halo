using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices.LAMS
{
    public interface IClientEngagementService
    {
        Task<ApiResponse> AddClientEngagement(HttpContext context, ClientEngagementReceivingDTO clientEngagementReceivingDTO);
        Task<ApiResponse> GetAllClientEngagement();
        Task<ApiResponse> GetClientEngagementById(long id);
        Task<ApiResponse> GetClientEngagementByName(string name);
        Task<ApiResponse> UpdateClientEngagement(HttpContext context, long id, ClientEngagementReceivingDTO clientEngagementReceivingDTO);
        Task<ApiResponse> DeleteClientEngagement(long id);

    }
}