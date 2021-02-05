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
        Task<ApiResponse> UpdateTaskFulfillment(HttpContext context, long id, TaskFulfillmentReceivingDTO taskFulfillmentReceivingDTO);
        Task<ApiResponse> DeleteTaskFulfillment(long id);
        Task<ApiResponse> SetIsPicked(HttpContext context, long id);
    }
}