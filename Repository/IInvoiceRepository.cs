using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HalobizMigrations.Models;

namespace HaloBiz.Repository
{
    public interface IInvoiceRepository
    {
        Task<Invoice> SaveInvoice(Invoice invoice);
        Task<IEnumerable<Invoice>> GetInvoice();
        Task<Invoice> UpdateInvoice(Invoice invoice);
        Task<bool> DeleteInvoice(Invoice invoice);
        Task<Invoice> FindInvoiceById(long Id);
        IQueryable<Invoice> GetInvoiceQueryiable();
        Task<IEnumerable<Invoice>> GetInvoiceByContractServiceId(long contactDivisionId);

    }
}