using System.Threading.Tasks;
using HaloBiz.DTOs.ReceivingDTOs.LAMS;
using Halobiz.Common.DTOs.ApiDTOs;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Halobiz.Common.DTOs.ReceivingDTOs;

namespace HaloBiz.MyServices.LAMS
{
    public interface IContractServiceForEndorsementService
    {
       // Task<ApiCommonResponse> AddNewContractServiceForEndorsement (HttpContext httpContext, ContractServiceForEndorsementReceivingDto contractServiceForEndorsementReceiving);
        Task<ApiCommonResponse> GetUnApprovedContractServiceForEndorsement();
        Task<ApiCommonResponse> GetEndorsementDetailsById(long endorsementId);
        Task<ApiCommonResponse> GetEndorsementServiceAddition(long endorsementId);

        Task<ApiCommonResponse> ApproveContractServiceForEndorsement(long Id, long sequence, bool isApproved, HttpContext httpContext);
        Task<ApiCommonResponse> JobPostingRenewContractService(HttpContext httpContext);
       // Task<ApiCommonResponse> ConvertContractServiceForEndorsement(HttpContext httpContext, long Id);
      //  Task<ApiCommonResponse> ConvertDebitCreditNoteEndorsement(HttpContext httpContext, long Id);
        Task<ApiCommonResponse> GetAllPossibleEndorsementStartDate(long contractServiceId);
        Task<ApiCommonResponse> GetEndorsementHistory(long contractServiceId);
        Task<ApiCommonResponse> AddNewRetentionContractServiceForEndorsement (HttpContext httpContext, List<ContractServiceForEndorsementReceivingDto> contractServiceForEndorsementDtos);
        Task<ApiCommonResponse> GetNewContractAdditionEndorsement(long customerDivisionId);
        Task<ApiCommonResponse> ConvertContractServiceForEndorsement(HttpContext httpContext, long Id);
    }
}