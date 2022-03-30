using HalobizMigrations.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnlinePortalBackend.DTOs.TransferDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Repository.Impl
{
    public class InvoiceRepository : IInvoiceRepository
    {

        private readonly HalobizContext _context;
        private readonly ILogger<ServicesRepo> _logger;
        public InvoiceRepository(HalobizContext context,
            ILogger<ServicesRepo> logger)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<ContractServiceInvoiceDTO> GetConractServiceInvoices(int contractServiceId)
        {
            var xx = _context.Invoices.Include(x => x.Receipts).Include(x => x.ContractService).Where(x => x.ContractServiceId == contractServiceId);
            IEnumerable<InvoiceDTO> invoices = _context.Invoices.Include(x => x.Receipts).Include(x => x.ContractService).Where(x => x.ContractServiceId == contractServiceId).Select(x => new InvoiceDTO
            {
                Id = (int)x.Id,
                VAT = x.ContractService.Vat.Value,
                InvoiceEndDate = x.EndDate,
                InvoiceStartDate = x.StartDate,
                InvoiceNumber = x.InvoiceNumber,
                InvoiceValue = x.Value,
                Payment = x.Receipts.FirstOrDefault(y => y.InvoiceId == x.Id).ReceiptValue,
                DateToBeSent = x.DateToBeSent,
                IsFinalInvoice = x.IsFinalInvoice.Value,
                IsReceiptedStatus = x.IsReceiptedStatus,
                InvoiceValueBalanceAfterReceipt = x.Receipts.FirstOrDefault(y => y.InvoiceId == x.Id).InvoiceValueBalanceAfterReceipting
            });

            if (invoices == null)
                return null;

            var serviceInvoices = new ContractServiceInvoiceDTO
            {
                Invoices = invoices,
                PaymentsOverDue = invoices.Where(x => x.IsFinalInvoice == true && x.IsReceiptedStatus == 0 && x.InvoiceEndDate < DateTime.Today).Count(),
                PaymentsDue = invoices.Where(x => x.DateToBeSent < DateTime.Today).Count(),
                TotalPayments = invoices.Where(x => x.Payment != null).Select(x => x.Payment.Value).Sum(),
                Status = invoices.Where(x => x.InvoiceValueBalanceAfterReceipt != null).Select(x => x.InvoiceValueBalanceAfterReceipt).Any(x => x.Value == 0) ? "Completed Payment" : "Outstanding"
            };

            return serviceInvoices;
        }
    }
}
