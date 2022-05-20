using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Halobiz.Common.Auths;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.ReceivingDTOs.LAMS;
using HaloBiz.DTOs.TransferDTOs.LAMS;
using HaloBiz.MyServices;
using HaloBiz.MyServices.LAMS;
using Microsoft.AspNetCore.Mvc;

namespace HaloBiz.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ModuleName(HalobizModules.ApprovalManagement,117)]

    public class ApprovalController : ControllerBase
    {
        private readonly IApprovalService _approvalService;

        public ApprovalController(IApprovalService approvalService)
        {
            this._approvalService = approvalService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetApproval()
        {
            return await _approvalService.GetAllApproval();
        }

        [HttpGet("GetPendingApprovals")]
        public async Task<ApiCommonResponse> GetPendingApprovals()
        {
            return await _approvalService.GetPendingApprovals();
        }

        [HttpGet("GetPendingApprovalsByServiceId/{serviceId}")]
        public async Task<ApiCommonResponse> GetPendingApprovalsByServiceId(long serviceId)
        {
            return await _approvalService.GetPendingApprovalsByServiceId(serviceId);
        }
        
        [HttpGet("GetApprovalsByServiceId/{serviceId}")]
        public async Task<ApiCommonResponse> GetApprovalsByServiceId(long serviceId)
        {
            return await _approvalService.GetApprovalsByServiceId(serviceId);
        }

        [HttpGet("GetPendingApprovalsByQuoteId/{quoteId}")]
        public async Task<ApiCommonResponse> GetPendingApprovalsByQuoteId(long quoteId)
        {
            return await _approvalService.GetPendingApprovalsByQuoteId(quoteId);
        }
        
        [HttpGet("GetApprovalsByQuoteId/{quoteId}")]
        public async Task<ApiCommonResponse> GetApprovalsByQuoteId(long quoteId)
        {
            return await _approvalService.GetApprovalsByQuoteId(quoteId);
        }

        [HttpGet("GetApprovalsByContractId/{contractId}")]
        public async Task<ApiCommonResponse> GetApprovalsByContractId(long contractId)
        {
            return await _approvalService.GetApprovalsByContractId(contractId);
        }

        [HttpGet("GetPendingApprovalsByContractId/{contractId}")]
        public async Task<ApiCommonResponse> GetPendingApprovalsByContractId(long contractId)
        {
            return await _approvalService.GetPendingApprovalsByContractId(contractId);
        }

        [HttpGet("GetApprovalsByEndorsementId/{endorsementId}")]
        public async Task<ApiCommonResponse> GetApprovalsByEndorsementId(long endorsementId)
        {
            return await _approvalService.GetApprovalsByEndorsementId(endorsementId);
        }

        [HttpGet("GetUserPendingApprovals")]
        public async Task<ApiCommonResponse> GetUserPendingApprovals()
        {
            return await _approvalService.GetUserPendingApprovals(HttpContext);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewApproval(ApprovalReceivingDTO approvalReceiving)
        {
            return await _approvalService.AddApproval(HttpContext, approvalReceiving);
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, ApprovalReceivingDTO approvalReceiving)
        {
            return await _approvalService.UpdateApproval(HttpContext, id, approvalReceiving);
        }

        [HttpPut("disapprove-or-approve-contractservice")]
        public async Task<ApiCommonResponse> approvedOrDisapprove(ContractApprovalDTO approvalReceiving)
        {
            return await _approvalService.ApprovalOrDispproveContractService(HttpContext, approvalReceiving);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _approvalService.DeleteApproval(id);
        }

        [HttpGet("GetApprovingLevelOfficeData")]
        public async Task<ApiCommonResponse> GetApprovingLevelOfficeData()
        {
            return await _approvalService.GetApprovingLevelOfficeData(HttpContext);
        }

    }
}