using OnlinePortalBackend.DTOs.ReceivingDTOs;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Repository
{
    public interface ISupplierRepository
    {
        Task<bool> AddNewAsset(AssetAdditionDTO request);
        Task<bool> PostTransactionForBooking(PostTransactionDTO request);
    }
}
