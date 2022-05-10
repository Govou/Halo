using Halobiz.Common.DTOs.ApiDTOs;
using Halobiz.Common.DTOs.ReceivingDTOs;
using System.Threading.Tasks;

namespace OnlinePortalBackend.MyServices.SecureMobilitySales
{
    public interface ISMSAccountService
    {
        Task<ApiCommonResponse> CreateIndividualAccount(SMSIndividualAccountDTO request);
        Task<ApiCommonResponse> CreateBusinessAccount(SMSBusinessAccountDTO request);
    }
}
