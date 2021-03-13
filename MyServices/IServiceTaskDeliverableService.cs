using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices
{
    public interface IServiceTaskDeliverableService
    {
        Task<ApiResponse> AddServiceTaskDeliverable(HttpContext context, ServiceTaskDeliverableReceivingDTO serviceTaskDeliverableReceivingDTO);
        Task<ApiResponse> GetAllServiceTaskDeliverables();
        Task<ApiResponse> GetServiceTaskDeliverableById(long id);
        Task<ApiResponse> GetServiceTaskDeliverableByName(string name);
        Task<ApiResponse> UpdateServiceTaskDeliverable(HttpContext context, long id, ServiceTaskDeliverableReceivingDTO serviceTaskDeliverableReceivingDTO);
        Task<ApiResponse> DeleteServiceTaskDeliverable(long id);
    }
}