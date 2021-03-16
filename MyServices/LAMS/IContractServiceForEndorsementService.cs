using System.Threading.Tasks;
using HaloBiz.DTOs.ReceivingDTOs.LAMS;
using HaloBiz.DTOs.ApiDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices.LAMS
{
    public interface IContractServiceForEndorsementService
    {
         Task<ApiResponse> AddNewContractServiceForEndorsement (HttpContext httpContext, ContractServiceForEndorsementReceivingDto contractServiceForEndorsementReceiving);
        Task<ApiResponse> GetUnApprovedContractServiceForEndorsement();
        Task<ApiResponse> ApproveContractServiceForEndorsement(long Id, bool isApproved);
        Task<ApiResponse> ConvertContractServiceForEndorsement(HttpContext httpContext, long Id);
        Task<ApiResponse> GetAllPossibleEndorsementStartDate(long contractServiceId);
    }
}