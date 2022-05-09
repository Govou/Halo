using Halobiz.Common.DTOs.ApiDTOs;
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
        public Task<ApiCommonResponse> ActivateWallet()
        {
            throw new System.NotImplementedException();
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
