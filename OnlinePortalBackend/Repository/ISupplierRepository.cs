using OnlinePortalBackend.DTOs.ReceivingDTOs;
using OnlinePortalBackend.DTOs.TransferDTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Repository
{
    public interface ISupplierRepository
    {
        Task<bool> AddNewAsset(AssetAdditionDTO request);
        Task<bool> PostTransactionForBooking(PostTransactionDTO request);
        Task<IEnumerable<VehicleMakeDTO>> GetVehicleMakes();
        Task<IEnumerable<VehicleModelDTO>> GetVehicleModels(int makeId);
        Task<IEnumerable<SupplierCategoryDTO>> GetSupplierCategories();
    }
}
