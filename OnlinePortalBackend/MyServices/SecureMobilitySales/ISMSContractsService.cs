using Halobiz.Common.DTOs.ApiDTOs;
using Halobiz.Common.DTOs.ReceivingDTOs;
using System.Threading.Tasks;

namespace OnlinePortalBackend.MyServices.SecureMobilitySales
{
    public interface ISMSContractsService
    {
        Task<ApiCommonResponse> CreateContract(SMSContractDTO contractDTO);
    }
}
