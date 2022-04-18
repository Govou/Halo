using HalobizMigrations.Models;
using OnlinePortalBackend.DTOs.TransferDTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Repository
{
    public interface IInvoiceRepository
    {
        Task<ContractInvoiceDTO> GetInvoices(long userId);
        Task<InvoiceDetailDTO> GetInvoice(int invoiceId);
    }
}
