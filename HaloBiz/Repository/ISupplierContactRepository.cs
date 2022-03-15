using System.Collections.Generic;
using System.Threading.Tasks;
using HaloBiz.Model;
using HalobizMigrations.Models;

namespace HaloBiz.Repository
{
    public interface ISupplierContactRepository
    {
        Task<SupplierContactMapping> SaveSupplierContact(SupplierContactMapping supplierContact);
        //Task<IEnumerable<SupplierContactMapping>> GetSupplierContacts();
        //Task<Supplier> UpdateSupplier(Supplier supplier);
        //Task<bool> DeleteSupplierContact(SupplierContactMapping supplierContact);
        //Task<SupplierContactMapping> FindSupplierContactById(long Id);
    }
}
