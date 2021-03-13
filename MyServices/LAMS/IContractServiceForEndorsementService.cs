using System.Threading.Tasks;
using HaloBiz.Data;
using AutoMapper;
using HaloBiz.DTOs.ReceivingDTOs.LAMS;
using HaloBiz.Model.LAMS;
using HaloBiz.DTOs.TransferDTOs.LAMS;
using HaloBiz.Repository.LAMS;
using HaloBiz.DTOs.ApiDTOs;

namespace HaloBiz.MyServices.LAMS
{
    public interface IContractServiceForEndorsementService
    {
         Task<ApiResponse> AddNewContractServiceForEndorsement (ContractServiceForEndorsementReceivingDto contractServiceForEndorsementReceiving);
        Task<ApiResponse> GetUnApprovedContractServiceForEndorsement();
        Task<ApiResponse> ApproveContractServiceForEndorsement(long Id, bool isApproved);
        Task<ApiResponse> ConvertContractServiceForEndorsement(long Id);
    }
}