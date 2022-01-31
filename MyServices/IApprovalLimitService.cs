using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices
{
    public interface IApprovalLimitService
    {
        Task<ApiCommonResponse> AddApprovalLimit(HttpContext context, ApprovalLimitReceivingDTO approvalLimitReceivingDTO);
        Task<ApiCommonResponse> GetAllApprovalLimit();
        Task<ApiCommonResponse> UpdateApprovalLimit(HttpContext context, long id, ApprovalLimitReceivingDTO approvalLimitReceivingDTO);
        Task<ApiCommonResponse> DeleteApprovalLimit(long id);
    }
}