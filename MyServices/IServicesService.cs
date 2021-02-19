using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices
{
    public interface IServicesService
    {
        Task<ApiResponse> AddService(ServicesReceivingDTO servicesReceivingDTO);
        Task<ApiResponse> GetAllServices();
        Task<ApiResponse> GetServiceById(long id);
        Task<ApiResponse> GetServiceByName(string name);
        Task<ApiResponse> UpdateServices(HttpContext context, long id, ServicesReceivingDTO serviceReceivingDTO);
        Task<ApiResponse> ApproveService(HttpContext context, long id);
        Task<ApiResponse> DisapproveService(HttpContext context, long id);
        Task<ApiResponse> RequestPublishService(HttpContext context, long id);

        Task<ApiResponse> DeleteService(long id);
        Task<ApiResponse> DeleteService(HttpContext context, long id);
        Task<ApiResponse> GetUnpublishedServices();
    }
}