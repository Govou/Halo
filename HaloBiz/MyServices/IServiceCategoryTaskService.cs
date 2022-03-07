using System.Threading.Tasks;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices
{
    public interface  IServiceCategoryTaskService
    {
        Task<ApiCommonResponse> AddServiceCategoryTask(HttpContext context, ServiceCategoryTaskReceivingDTO serviceCategoryTaskReceivingDTO);
        Task<ApiCommonResponse> GetAllServiceCategoryTasks();
        Task<ApiCommonResponse> GetServiceCategoryTaskById(long id);
        Task<ApiCommonResponse> GetServiceCategoryTaskByName(string name);
        Task<ApiCommonResponse> UpdateServiceCategoryTask(HttpContext context, long id, ServiceCategoryTaskReceivingDTO serviceCategoryTaskReceivingDTO);
        Task<ApiCommonResponse> DeleteServiceCategoryTask(long id);
    }
}