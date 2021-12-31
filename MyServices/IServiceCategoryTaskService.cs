using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices
{
    public interface  IServiceCategoryTaskService
    {
        Task<ApiResponse> AddServiceCategoryTask(HttpContext context, ServiceCategoryTaskReceivingDTO serviceCategoryTaskReceivingDTO);
        Task<ApiResponse> GetAllServiceCategoryTasks();
        Task<ApiResponse> GetServiceCategoryTaskById(long id);
        Task<ApiResponse> GetServiceCategoryTaskByName(string name);
        Task<ApiResponse> UpdateServiceCategoryTask(HttpContext context, long id, ServiceCategoryTaskReceivingDTO serviceCategoryTaskReceivingDTO);
        Task<ApiResponse> DeleteServiceCategoryTask(long id);
    }
}