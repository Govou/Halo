using System.Collections.Generic;
using System.Threading.Tasks;
using HaloBiz.Model;

namespace HaloBiz.Repository
{
    public interface ISupplierRepository
    {
        Task<Supplier> SaveSupplier(Supplier supplier);
        Task<IEnumerable<Supplier>> GetSuppliers();
        Task<Supplier> UpdateSupplier(Supplier supplier);
        Task<bool> DeleteSupplier(Supplier supplier);
        Task<Supplier> FindSupplierById(long Id);
    }
}