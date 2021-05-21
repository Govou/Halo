using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices.LAMS
{
    public interface ITaskFulfillmentService
    {
        Task<ApiResponse> AddTaskFulfillment(HttpContext context, TaskFulfillmentReceivingDTO taskFulfillmentReceivingDTO);
        Task<ApiResponse> GetAllTaskFulfillment();
        Task<ApiResponse> GetTaskFulfillmentById(long id);
        Task<ApiResponse> GetTaskFulfillmentByName(string name);
        Task<ApiResponse> GetAllUnCompletedTaskFulfillmentForTaskOwner(long taskOwnerId);
        Task<ApiResponse> GetAllTaskFulfillmentForTaskOwner(long taskOwnerId);
        Task<ApiResponse> GetPMWidgetStatistics(long taskOwnerId);
        Task<ApiResponse> UpdateTaskFulfillment(HttpContext context, long id, TaskFulfillmentReceivingDTO taskFulfillmentReceivingDTO);
        Task<ApiResponse> DeleteTaskFulfillment(long id);
        Task<ApiResponse> GetTaskFulfillmentsByOperatingEntityHeadId(long id);
        Task<ApiResponse> GetTaskFulfillmentDetails(long id);
        Task<ApiResponse> GetTaskDeliverableSummary(long responsibleId);
        Task<ApiResponse> SetIsPicked(HttpContext context, long id, bool isPicked);
    }
}