using System.Threading.Tasks;
using HaloBiz.DTOs.ReceivingDTOs.LAMS;
using Halobiz.Common.DTOs.ApiDTOs;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace HaloBiz.MyServices.LAMS
{
    public interface IContractServiceForEndorsementService
    {
       // Task<ApiCommonResponse> AddNewContractServiceForEndorsement (HttpContext httpContext, ContractServiceForEndorsementReceivingDto contractServiceForEndorsementReceiving);
        Task<ApiCommonResponse> GetUnApprovedContractServiceForEndorsement();
        Task<ApiCommonResponse> GetEndorsementDetailsById(long endorsementId);
        Task<ApiCommonResponse> ApproveContractServiceForEndorsement(long Id, long sequence, bool isApproved);
        Task<ApiCommonResponse> JobPostingRenewContractService(HttpContext httpContext);
        Task<ApiCommonResponse> ConvertContractServiceForEndorsement(HttpContext httpContext, long Id);
        Task<ApiCommonResponse> ConvertDebitCreditNoteEndorsement(HttpContext httpContext, long Id);
        Task<ApiCommonResponse> GetAllPossibleEndorsementStartDate(long contractServiceId);
        Task<ApiCommonResponse> GetEndorsementHistory(long contractServiceId);
        Task<ApiCommonResponse> AddNewRetentionContractServiceForEndorsement (HttpContext httpContext, List<ContractServiceForEndorsementReceivingDto> contractServiceForEndorsementDtos);
    }
}