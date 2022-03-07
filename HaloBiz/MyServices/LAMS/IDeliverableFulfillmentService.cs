using System.Threading.Tasks;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.ReceivingDTOs.LAMS;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices.LAMS
{
    public interface IDeliverableFulfillmentService
    {
        Task<ApiCommonResponse> AddDeliverableFulfillment(HttpContext context, DeliverableFulfillmentReceivingDTO deliverableFulfillmentReceivingDTO);
        Task<ApiCommonResponse> GetAllDeliverableFulfillment();
        Task<ApiCommonResponse> DeliverableToAssignedUserRatio(long taskMasterId);
        Task<ApiCommonResponse> GetDeliverableFulfillmentById(long id);
        Task<ApiCommonResponse> GetDeliverableFulfillmentByName(string name);
        Task<ApiCommonResponse> UpdateDeliverableFulfillment(HttpContext context, long id, DeliverableFulfillmentReceivingDTO deliverableFulfillmentReceivingDTO);
        Task<ApiCommonResponse> DeleteDeliverableFulfillment(long id);        
        Task<ApiCommonResponse> ReAssignDeliverableFulfillment(HttpContext context, long id, DeliverableFulfillmentReceivingDTO deliverableFulfillmentReceivingDTO);
        Task<ApiCommonResponse> SetIsPicked(HttpContext context, long id);
        Task<ApiCommonResponse> SetRequestedForValidation(HttpContext context, long id,  DeliverableFulfillmentApprovalReceivingDTO dto);
        Task<ApiCommonResponse> SetDeliveredStatus(HttpContext context, long id);
        Task<ApiCommonResponse> GetUserDeliverableFulfillmentStat(long userId);
        Task<ApiCommonResponse> GetUserDeliverableFulfillmentDashboard(long userId);
    }
}