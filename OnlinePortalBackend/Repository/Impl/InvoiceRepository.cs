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

        public async Task<ContractInvoiceDTO> GetInvoices(int userId)
        {
            var serviceInvoices = new List<ContractServiceInvoiceDTO>();
            var contractInvoice = new ContractInvoiceDTO();

            IEnumerable<InvoiceDTO> invoices = _context.Invoices.Include(x => x.Receipts).Include(x => x.ContractService).Where(x => x.CustomerDivisionId == userId && x.IsReceiptedStatus != 2).Select(x => new InvoiceDTO
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
                ContractId = (int)x.ContractId,
                InvoiceValueBalanceAfterReceipt = x.Receipts.FirstOrDefault(y => y.InvoiceId == x.Id).InvoiceValueBalanceAfterReceipting,
            });

            if (invoices == null)
                return null;

            var grpInvoices = invoices.GroupBy(x => x.ContractId);

            foreach (var item in grpInvoices)
            {
                serviceInvoices.Add(new ContractServiceInvoiceDTO
                {
                    ContractId = item.Key,
                    Invoices = invoices.OrderBy(x => x.DateToBeSent),
                    PaymentsOverDue = invoices.Where(x => x.IsFinalInvoice == true && x.IsReceiptedStatus == 0 && x.InvoiceEndDate < DateTime.Today).Count(),
                    PaymentsDue = invoices.Where(x => x.DateToBeSent < DateTime.Today).Count(),
                    TotalPayments = invoices.Where(x => x.Payment != null).Select(x => x.Payment.Value).Sum(),
                    Status = invoices.Where(x => x.InvoiceValueBalanceAfterReceipt != null).Any(x => x.IsReceiptedStatus != 2) ? "Outstanding" : "Completed Payment"
                });
            }

            if (serviceInvoices.Count > 0)
            {
               contractInvoice = new ContractInvoiceDTO { ContractServiceInvoices = serviceInvoices };
            }
            

            return contractInvoice;
        }

        public async Task<InvoiceDetailDTO> GetInvoice(int invoiceId)
        {
            var firstInvoice = _context.Invoices.FirstOrDefault(x => x.Id == invoiceId);
            var allInvoices = new List<InvoiceDetailInfo>();
            var InvoiceDTO = new InvoiceDetailDTO();
            var totalInvoices = 0.0;
            if (firstInvoice == null)
                return null;

            if (firstInvoice.InvoiceNumber.StartsWith("G"))
            {
                var groupedInvoices = _context.Invoices.Where(x => x.GroupInvoiceNumber == firstInvoice.GroupInvoiceNumber && x.DateToBeSent.Date == firstInvoice.DateToBeSent.Date).Select(
                    x => new InvoiceDetailInfo
                    {
                        Total = x.Value,
                        ContractServiceId = (int)x.ContractServiceId,
                        InvoiceNumber = x.InvoiceNumber
                    }).ToList();
                var conServices = new Dictionary<int, string>();
                foreach (var invoice in groupedInvoices)
                {
                    var res = _context.ContractServices.Include(x => x.Service).FirstOrDefault(x => x.Id == invoice.ContractServiceId);
                    try{ conServices.Add(invoice.ContractServiceId, res.Service.Name); }catch (Exception) { }
                    invoice.Quantity = (int)res.Quantity;
                    invoice.Discount = res.Discount;
                }

                foreach (var item in groupedInvoices)
                {
                    item.ServiceName = conServices[item.ContractServiceId];
                    totalInvoices += item.Total;
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
            InvoiceDTO.InvoiceBalanceBeforeReceipting = firstInvoice.Value;
            InvoiceDTO.InvoiceValue = totalInvoices;
            InvoiceDTO.InvoiceDetailsInfos = allInvoices;

            return InvoiceDTO;
        }
    }
}
