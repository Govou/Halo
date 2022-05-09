using System.Threading.Tasks;

namespace OnlinePortalBackend.Repository
{
    public interface IWalletRepository
    {
        Task<bool> ActivateWallet();
        Task<bool> LoadWallet();
        Task<bool> SpendWallet();
    }
}
