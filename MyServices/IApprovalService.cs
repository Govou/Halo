using System.Collections.Generic;
using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HalobizMigrations.Models;

using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices
{
    public interface IApprovalService
    {
        Task<ApiCommonResponse> AddApproval(HttpContext context, ApprovalReceivingDTO approvalReceivingDTO);
        Task<ApiCommonResponse> GetAllApproval();
        Task<ApiCommonResponse> UpdateApproval(HttpContext context, long id, ApprovalReceivingDTO approvalReceivingDTO);
        Task<ApiCommonResponse> DeleteApproval(long id);
        Task<ApiCommonResponse> GetPendingApprovals();
        Task<ApiCommonResponse> GetUserPendingApprovals(HttpContext httpContext);
        Task<bool> SetUpApprovalsForServiceCreation(Service service, HttpContext httpContext);
        Task<bool> SetUpApprovalsForClientCreation(long id, HttpContext httpContext);
        Task<bool> SetUpApprovalsForContractModificationEndorsement(ContractServiceForEndorsement contractServiceForEndorsement, HttpContext httpContext);
        Task<bool> SetUpApprovalsForContractRenewalEndorsement(List<ContractServiceForEndorsement> contractServiceForEndorsements, HttpContext httpContext);
        Task<ApiCommonResponse> GetPendingApprovalsByServiceId(long serviceId);
        Task<ApiCommonResponse> GetApprovalsByServiceId(long serviceId);
        Task<ApiCommonResponse> GetPendingApprovalsByQuoteId(long quoteId);
        Task<ApiCommonResponse> GetApprovalsByEndorsementId(long endorsement);
        Task<ApiCommonResponse> GetApprovalsByQuoteId(long quoteId);
    }
}