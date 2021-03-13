using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;

namespace HaloBiz.MyServices.LAMS
{
    public interface IContractServiceService
    {
        Task<ApiResponse> DeleteContractService(long id);

        Task<ApiResponse> GetAllContractsServcieForAContract(long contractId);

        Task<ApiResponse> GetContractServiceByReferenceNumber(string refNo);

        Task<ApiResponse> GetContractServiceById(long id);
    }
}