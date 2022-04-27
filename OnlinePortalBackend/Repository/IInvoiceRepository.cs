using HalobizMigrations.Models;
using OnlinePortalBackend.DTOs.TransferDTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace OnlinePortalBackend.Repository
{
    public interface IInvoiceRepository
    {
        Task<ContractInvoiceDTO> GetInvoices(long userId);
        Task<InvoiceDetailDTO> GetInvoice(string invoiceNumber, DateTime invoiceDate);
        Task<bool> CheckIfInvoiceHasBeenPaid(string invoiceNumber, string sessionId, int userId);
        
    }
}
