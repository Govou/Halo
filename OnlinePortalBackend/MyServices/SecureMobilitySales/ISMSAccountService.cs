using Halobiz.Common.DTOs.ApiDTOs;
using Halobiz.Common.DTOs.ReceivingDTOs;
using OnlinePortalBackend.DTOs.ReceivingDTOs;
using System.Threading.Tasks;

namespace OnlinePortalBackend.MyServices.SecureMobilitySales
{
    public interface ISMSAccountService
    {
        Task<ApiCommonResponse> CreateIndividualAccount(SMSIndividualAccountDTO request);
        Task<ApiCommonResponse> CreateBusinessAccount(SMSBusinessAccountDTO request);
        Task<ApiCommonResponse> GetCustomerProfile(int profileId);
        Task<ApiCommonResponse> CreateSupplierAccount(SMSSupplierAccountDTO request);
    }
}
