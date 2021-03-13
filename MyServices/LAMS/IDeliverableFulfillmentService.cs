using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.ReceivingDTOs.LAMS;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices.LAMS
{
    public interface IDeliverableFulfillmentService
    {
        Task<ApiResponse> AddDeliverableFulfillment(HttpContext context, DeliverableFulfillmentReceivingDTO deliverableFulfillmentReceivingDTO);
        Task<ApiResponse> GetAllDeliverableFulfillment();
        Task<ApiResponse> DeliverableToAssignedUserRatio(long taskMasterId);
        Task<ApiResponse> GetDeliverableFulfillmentById(long id);
        Task<ApiResponse> GetDeliverableFulfillmentByName(string name);
        Task<ApiResponse> UpdateDeliverableFulfillment(HttpContext context, long id, DeliverableFulfillmentReceivingDTO deliverableFulfillmentReceivingDTO);
        Task<ApiResponse> DeleteDeliverableFulfillment(long id);        
        Task<ApiResponse> ReAssignDeliverableFulfillment(HttpContext context, long id, DeliverableFulfillmentReceivingDTO deliverableFulfillmentReceivingDTO);
        Task<ApiResponse> SetIsPicked(HttpContext context, long id);
        Task<ApiResponse> SetRequestedForValidation(HttpContext context, long id,  DeliverableFulfillmentApprovalReceivingDTO dto);
        Task<ApiResponse> SetDeliveredStatus(HttpContext context, long id);
        Task<ApiResponse> GetUserDeliverableFulfillmentStat(long userId);
    }
}