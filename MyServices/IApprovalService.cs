using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.Model;
using HaloBiz.Model.LAMS;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices
{
    public interface IApprovalService
    {
        Task<ApiResponse> AddApproval(HttpContext context, ApprovalReceivingDTO approvalReceivingDTO);
        Task<ApiResponse> GetAllApproval();
        Task<ApiResponse> UpdateApproval(HttpContext context, long id, ApprovalReceivingDTO approvalReceivingDTO);
        Task<ApiResponse> DeleteApproval(long id);
        Task<ApiResponse> GetPendingApprovals();
        Task<ApiResponse> GetUserPendingApprovals(HttpContext httpContext);
        Task<bool> SetUpApprovalsForServiceCreation(Services service, HttpContext httpContext);
        Task<bool> SetUpApprovalsForClientCreation(Lead lead, HttpContext httpContext);
        Task<bool> SetUpApprovalsForEndorsement(CustomerDivision customerDivision, HttpContext httpContext);
    }
}