using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices
{
    public interface IApprovalLimitService
    {
        Task<ApiResponse> AddApprovalLimit(HttpContext context, ApprovalLimitReceivingDTO approvalLimitReceivingDTO);
        Task<ApiResponse> GetAllApprovalLimit();
        Task<ApiResponse> UpdateApprovalLimit(HttpContext context, long id, ApprovalLimitReceivingDTO approvalLimitReceivingDTO);
        Task<ApiResponse> DeleteApprovalLimit(long id);
    }
}