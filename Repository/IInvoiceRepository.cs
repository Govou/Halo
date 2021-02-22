using System.Collections.Generic;
using System.Threading.Tasks;
using HaloBiz.Model.AccountsModel;

namespace HaloBiz.Repository
{
    public interface IInvoiceRepository
    {
        Task<Invoice> SaveInvoice(Invoice invoice);
        Task<IEnumerable<Invoice>> GetInvoice();
        Task<Invoice> UpdateInvoice(Invoice invoice);
        Task<bool> DeleteInvoice(Invoice invoice);
        Task<Invoice> FindInvoiceById(long Id);

    }
}