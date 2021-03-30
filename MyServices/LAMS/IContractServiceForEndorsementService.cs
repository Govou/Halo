using System.Threading.Tasks;
using HaloBiz.DTOs.ReceivingDTOs.LAMS;
using HaloBiz.DTOs.ApiDTOs;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace HaloBiz.MyServices.LAMS
{
    public interface IContractServiceForEndorsementService
    {
        Task<ApiResponse> AddNewContractServiceForEndorsement (HttpContext httpContext, ContractServiceForEndorsementReceivingDto contractServiceForEndorsementReceiving);
        Task<ApiResponse> GetUnApprovedContractServiceForEndorsement();
        Task<ApiResponse> GetEndorsementDetailsById(long endorsementId);
        Task<ApiResponse> ApproveContractServiceForEndorsement(long Id, long sequence, bool isApproved);
        Task<ApiResponse> ConvertContractServiceForEndorsement(HttpContext httpContext, long Id);
        Task<ApiResponse> GetAllPossibleEndorsementStartDate(long contractServiceId);
        Task<ApiResponse> AddNewRetentionContractServiceForEndorsement (HttpContext httpContext, List<ContractServiceForEndorsementReceivingDto> contractServiceForEndorsementDtos);
    }
}