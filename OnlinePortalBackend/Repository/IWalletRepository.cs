using OnlinePortalBackend.DTOs.ReceivingDTOs;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Repository
{
    public interface IWalletRepository
    {
        Task<(bool, string)> ActivateWallet(ActivateWalletDTO request);
        Task<bool> LoadWallet();
        Task<bool> SpendWallet();
    }
}
