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

        public async Task<ContractServiceInvoiceDTO> GetInvoices(int userId, int? contractService, int? contractId, int limit = 10)
        {
            ContractServiceInvoiceDTO serviceInvoices = null;

            if (contractService != null && contractService.Value == 0)
            {

                IEnumerable<InvoiceDTO> invoices = _context.Invoices.Include(x => x.Receipts).Include(x => x.ContractService).Where(x => x.ContractServiceId == contractService && x.CustomerDivisionId == userId && x.IsDeleted == false).Select(x => new InvoiceDTO
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

                serviceInvoices = new ContractServiceInvoiceDTO
                {
                    Invoices = invoices.Take(limit),
                    PaymentsOverDue = invoices.Where(x => x.IsFinalInvoice == true && x.IsReceiptedStatus == 0 && x.InvoiceEndDate < DateTime.Today).Count(),
                    PaymentsDue = invoices.Where(x => x.DateToBeSent < DateTime.Today).Count(),
                    TotalPayments = invoices.Where(x => x.Payment != null).Select(x => x.Payment.Value).Sum(),
                    Status = invoices.Where(x => x.InvoiceValueBalanceAfterReceipt != null).Select(x => x.InvoiceValueBalanceAfterReceipt).Any(x => x.Value == 0) ? "Completed Payment" : "Outstanding"
                };
            }

            else if (contractId != null && contractId.Value == 0)
            {
                IEnumerable<InvoiceDTO> invoices = _context.Invoices.Include(x => x.Receipts).Include(x => x.ContractService).Where(x => x.ContractServiceId == contractService && x.CustomerDivisionId == userId).Select(x => new InvoiceDTO
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

                serviceInvoices = new ContractServiceInvoiceDTO
                {
                    Invoices = invoices.Take(limit),
                    PaymentsOverDue = invoices.Where(x => x.IsFinalInvoice == true && x.IsReceiptedStatus == 0 && x.InvoiceEndDate < DateTime.Today).Count(),
                    PaymentsDue = invoices.Where(x => x.DateToBeSent < DateTime.Today).Count(),
                    TotalPayments = invoices.Where(x => x.Payment != null).Select(x => x.Payment.Value).Sum(),
                    Status = invoices.Where(x => x.InvoiceValueBalanceAfterReceipt != null).Select(x => x.InvoiceValueBalanceAfterReceipt).Any(x => x.Value == 0) ? "Completed Payment" : "Outstanding"
                };
            }

            else
            {
                IEnumerable<InvoiceDTO> invoices = _context.Invoices.Include(x => x.Receipts).Include(x => x.ContractService).Where(x => x.CustomerDivisionId == userId).Select(x => new InvoiceDTO
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

                serviceInvoices = new ContractServiceInvoiceDTO
                {
                    Invoices = invoices.Take(limit),
                    PaymentsOverDue = invoices.Where(x => x.IsFinalInvoice == true && x.IsReceiptedStatus == 0 && x.InvoiceEndDate < DateTime.Today).Count(),
                    PaymentsDue = invoices.Where(x => x.DateToBeSent < DateTime.Today).Count(),
                    TotalPayments = invoices.Where(x => x.Payment != null).Select(x => x.Payment.Value).Sum(),
                    Status = invoices.Where(x => x.InvoiceValueBalanceAfterReceipt != null).Select(x => x.InvoiceValueBalanceAfterReceipt).Any(x => x.Value == 0) ? "Completed Payment" : "Outstanding"
                };
            }

            return serviceInvoices;
        }

        public async Task<InvoiceDetailDTO> GetInvoice(int invoiceId)
        {
            var firstInvoice = _context.Invoices.FirstOrDefault(x => x.Id == invoiceId);
            var allInvoices = new List<InvoiceDetailInfo>();
            var InvoiceDTO = new InvoiceDetailDTO();
            if (firstInvoice == null)
                return null;

            if (firstInvoice.InvoiceNumber.StartsWith("G"))
            {
                var groupedInvoices = _context.Invoices.Where(x => x.GroupInvoiceNumber == firstInvoice.GroupInvoiceNumber && x.DateToBeSent.Date == firstInvoice.DateToBeSent.Date).Select(
                    x => new InvoiceDetailInfo
                    {
                        Total = x.Value,
                        ContractServiceId = (int)x.ContractServiceId
                    }).ToList();
                var conServices = new Dictionary<int, string>();
                foreach (var invoice in groupedInvoices)
                {
                    var res = _context.ContractServices.Include(x => x.Service).FirstOrDefault(x => x.Id == invoice.ContractServiceId);
                    conServices.Add(invoice.ContractServiceId, res.Service.Name );
                    invoice.Quantity = (int)res.Quantity;
                    invoice.Discount = res.Discount;
                }

                foreach (var item in groupedInvoices)
                {
                    item.ServiceName = conServices[item.ContractServiceId];
                }
                allInvoices = groupedInvoices;
            }
            else
            {
                var invoice = new InvoiceDetailInfo
                {
                    Total = firstInvoice.Value,
                    ContractServiceId = (int)firstInvoice.ContractServiceId
                };
                var res = _context.ContractServices.Include(x => x.Service).FirstOrDefault(x => x.Id == invoice.ContractServiceId);
                invoice.ServiceName = res.Service.Name;
                invoice.Quantity = (int)res.Quantity;
                invoice.Discount = res.Discount;
                allInvoices.Add(invoice);
            }

            InvoiceDTO.InvoiceNumber = firstInvoice.InvoiceNumber;
            InvoiceDTO.InvoiceStart = firstInvoice.StartDate;
            InvoiceDTO.InvoiceEnd = firstInvoice.EndDate;
            InvoiceDTO.InvoiceDue = firstInvoice.DateToBeSent;
            InvoiceDTO.InvoiceDetailsInfos = allInvoices;

            return InvoiceDTO;
        }
    }
}
