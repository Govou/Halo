using System.Threading.Tasks;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices.LAMS
{
    public interface ITaskFulfillmentService
    {
        Task<ApiCommonResponse> AddTaskFulfillment(HttpContext context, TaskFulfillmentReceivingDTO taskFulfillmentReceivingDTO);
        Task<ApiCommonResponse> GetAllTaskFulfillment();
        Task<ApiCommonResponse> GetTaskFulfillmentById(long id);
        Task<ApiCommonResponse> GetTaskFulfillmentByName(string name);
        Task<ApiCommonResponse> GetAllUnCompletedTaskFulfillmentForTaskOwner(long taskOwnerId);
        Task<ApiCommonResponse> GetAllTaskFulfillmentForTaskOwner(long taskOwnerId);
        Task<ApiCommonResponse> GetPMWidgetStatistics(long taskOwnerId);
        Task<ApiCommonResponse> UpdateTaskFulfillment(HttpContext context, long id, TaskFulfillmentReceivingDTO taskFulfillmentReceivingDTO);
        Task<ApiCommonResponse> DeleteTaskFulfillment(long id);
        Task<ApiCommonResponse> GetTaskFulfillmentsByOperatingEntityHeadId(long id);
        Task<ApiCommonResponse> GetTaskFulfillmentDetails(long id);
        Task<ApiCommonResponse> GetTaskDeliverableSummary(long responsibleId);
        Task<ApiCommonResponse> SetIsPicked(HttpContext context, long id, bool isPicked);
    }
}