using OnlinePortalBackend.DTOs.ReceivingDTOs;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Repository
{
    public interface IWalletRepository
    {
        Task<(bool isSuccess, string message)> ActivateWallet(ActivateWalletDTO request);
        Task<(bool isSuccess, string message)> LoadWallet(LoadWalletDTO request);
        Task<(bool isSuccess, SpendWalletResponseDTO message)> SpendWallet(SpendWalletDTO request);
        Task<(bool isSuccess, object message)> GetWalletBalance(int profileId);
        Task<(bool isSuccess, bool status)> GetWalletActivationStatus(int profileId);
    }
}
