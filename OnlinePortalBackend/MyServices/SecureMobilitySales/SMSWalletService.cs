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

        public async Task<ApiCommonResponse> GetWalletBalance(int profileId)
        {
            var result = await _walletRepository.GetWalletBalance(profileId);
            if (!result.isSuccess)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, result.message);
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS, result.message);
        }

        public async Task<ApiCommonResponse> LoadWallet(LoadWalletDTO request)
        {
            var result = await _walletRepository.LoadWallet(request);
            if (!result.isSuccess)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, result.message);
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS, result.message);
        }

        public async Task<ApiCommonResponse> SpendWallet(SpendWalletDTO request)
        {
            var result = await _walletRepository.SpendWallet(request);
            if (!result.isSuccess)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, result.message);
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS, result.message);
        }
    }
}
