using System.Collections.Generic;
using System.Threading.Tasks;
using HaloBiz.Model;

namespace HaloBiz.Repository
{
    public interface ISupplierServiceRepository
    {
        Task<SupplierService> SaveSupplierService(SupplierService supplier);
        Task<IEnumerable<SupplierService>> GetSupplierServices();
        Task<SupplierService> UpdateSupplierService(SupplierService supplier);
        Task<bool> DeleteSupplierService(SupplierService supplier);
        Task<SupplierService> FindSupplierServiceById(long Id);
    }
}