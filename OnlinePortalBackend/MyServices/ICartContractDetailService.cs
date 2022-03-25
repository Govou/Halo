using Halobiz.Common.DTOs.ApiDTOs;
using OnlinePortalBackend.DTOs.TransferDTOs;
using System.Threading.Tasks;

namespace OnlinePortalBackend.MyServices
{
    public interface ICartContractDetailService
    {
        Task<ApiCommonResponse> GetCartContractServiceById(long id);
        Task<ApiCommonResponse> GetAllContractsServcieForAContract(long contractId);
    }
}
