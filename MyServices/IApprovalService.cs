using System.Collections.Generic;
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
        Task<bool> SetUpApprovalsForClientCreation(long id, HttpContext httpContext);
        Task<bool> SetUpApprovalsForContractModificationEndorsement(ContractServiceForEndorsement contractServiceForEndorsement, HttpContext httpContext);
        Task<bool> SetUpApprovalsForContractRenewalEndorsement(List<ContractServiceForEndorsement> contractServiceForEndorsements, HttpContext httpContext);
        Task<ApiResponse> GetPendingApprovalsByServiceId(long serviceId);
        Task<ApiResponse> GetApprovalsByServiceId(long serviceId);
        Task<ApiResponse> GetPendingApprovalsByQuoteId(long quoteId);
        Task<ApiResponse> GetApprovalsByEndorsementId(long endorsement);
        Task<ApiResponse> GetApprovalsByQuoteId(long quoteId);
    }
}