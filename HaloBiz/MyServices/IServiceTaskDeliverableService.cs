using System.Threading.Tasks;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices
{
    public interface IServiceTaskDeliverableService
    {
        Task<ApiCommonResponse> AddServiceTaskDeliverable(HttpContext context, ServiceTaskDeliverableReceivingDTO serviceTaskDeliverableReceivingDTO);
        Task<ApiCommonResponse> GetAllServiceTaskDeliverables();
        Task<ApiCommonResponse> GetServiceTaskDeliverableById(long id);
        Task<ApiCommonResponse> GetServiceTaskDeliverableByName(string name);
        Task<ApiCommonResponse> UpdateServiceTaskDeliverable(HttpContext context, long id, ServiceTaskDeliverableReceivingDTO serviceTaskDeliverableReceivingDTO);
        Task<ApiCommonResponse> DeleteServiceTaskDeliverable(long id);
    }
}