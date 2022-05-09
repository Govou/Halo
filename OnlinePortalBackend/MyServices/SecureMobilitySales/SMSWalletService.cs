using Halobiz.Common.DTOs.ApiDTOs;
using OnlinePortalBackend.DTOs.ReceivingDTOs;
using OnlinePortalBackend.Repository;
using System.Threading.Tasks;

namespace OnlinePortalBackend.MyServices.SecureMobilitySales
{
    public class SMSWalletService : ISMSWalletService
    {
        private readonly IWalletRepository _walletRepository;
        public SMSWalletService(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
        }
        public async Task<ApiCommonResponse> ActivateWallet(ActivateWalletDTO request)
        {
            var result = await _walletRepository.ActivateWallet(request);
            if (!result.isSuccess)
            {
               return  CommonResponse.Send(ResponseCodes.FAILURE, result.message);
            }
           
            return CommonResponse.Send(ResponseCodes.SUCCESS, result.message);
        }

        public Task<ApiCommonResponse> LoadWallet()
        {
            throw new System.NotImplementedException();
        }

        public Task<ApiCommonResponse> SpendWallet()
        {
            throw new System.NotImplementedException();
        }
    }
}
