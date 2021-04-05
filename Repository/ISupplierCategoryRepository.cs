using System.Collections.Generic;
using System.Threading.Tasks;
using HaloBiz.Model;

namespace HaloBiz.Repository
{
    public interface ISupplierCategoryRepository
    {
        Task<SupplierCategory> SaveSupplierCategory(SupplierCategory supplierCategory);
        Task<IEnumerable<SupplierCategory>> GetSupplierCategories();
        Task<SupplierCategory> UpdateSupplierCategory(SupplierCategory supplierCategory);
        Task<bool> DeleteSupplierCategory(SupplierCategory supplierCategory);
        Task<SupplierCategory> FindSupplierCategoryById(long Id);
    }
}