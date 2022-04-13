using OnlinePortalBackend.DTOs.TransferDTOs;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Repository
{
    public interface IInvoiceRepository
    {
        Task<ContractInvoiceDTO> GetInvoices(int userId);
        Task<InvoiceDetailDTO> GetInvoice(int invoiceId);
    }
}
