using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices
{
    public interface IServicesService
    {
        Task<ApiCommonResponse> AddService(HttpContext context, ServiceReceivingDTO servicesReceivingDTO);
        Task<ApiCommonResponse> GetAllServices();
        Task<ApiCommonResponse> GetOnlinePortalServices();
        Task<ApiCommonResponse> GetServiceById(long id);
        Task<ApiCommonResponse> GetServiceByName(string name);
        Task<ApiCommonResponse> UpdateServices(HttpContext context, long id, ServiceReceivingDTO serviceReceivingDTO);
        Task<ApiCommonResponse> ApproveService(HttpContext context, long id, long sequence);
        Task<ApiCommonResponse> DisapproveService(HttpContext context, long id, long sequence);
        Task<ApiCommonResponse> RequestPublishService(HttpContext context, long id);
        Task<ApiCommonResponse> DeleteService(long id);
        Task<ApiCommonResponse> DeleteService(HttpContext context, long id);
        Task<ApiCommonResponse> GetUnpublishedServices();
    }
}