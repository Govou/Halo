using AutoMapper;
using Halobiz.Common.Helpers;
using HalobizMigrations.Data;
using HalobizMigrations.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnlinePortalBackend.DTOs.TransferDTOs;
using OnlinePortalBackend.Helpers;
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
        private readonly IMapper _mapper;
        public InvoiceRepository(HalobizContext context,
            ILogger<ServicesRepo> logger,
            IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        public async Task<ContractInvoiceDTO> GetInvoices(long userId)
        {

            var invoices = new ContractInvoiceDTO();
            var contractInvoices = _context.Invoices.Where(x => x.CustomerDivisionId == userId).Select(x => x.ContractServiceId).Distinct();

            foreach (var item in contractInvoices)
            {
                invoices = await GetInvoiceByContractServiceId(item);
            }

           return invoices;

           
        }

        public async Task<InvoiceDetailDTO> GetInvoice(int invoiceId)
        {
            var contractService = _context.Invoices.FirstOrDefault(x => x.Id == invoiceId);
            var invoices = await GetInvoiceByContractServiceId(contractService.ContractServiceId);

            var contractServiceInvoice = invoices.ContractServiceInvoices.FirstOrDefault(x => x.ContractId == contractService.ContractId);
            var grpContractServiceInvoice = contractServiceInvoice.Invoices.Where(x => x.InvoiceStartDate == contractService.StartDate);
            var contractInvoice = invoices.ContractServiceInvoices.FirstOrDefault(x => x.ContractId == contractService.ContractId);
            var invoiceDetailsInfo = new List<InvoiceDetailInfo>();
            var receipt = _context.Receipts.Include(x => x.Invoice).Where(x => x.InvoiceId == invoiceId).OrderByDescending(x => x.Id).FirstOrDefault();

            foreach (var item in grpContractServiceInvoice)
            {
                invoiceDetailsInfo.Add(new InvoiceDetailInfo
                {
                    ContractServiceId = (int)contractService.ContractServiceId,
                    InvoiceNumber = item.InvoiceNumber,
                    Discount = contractService.Discount,
                    Quantity = (int)contractService.Quantity,
                    ServiceName = item.ServiceName,  //contractService.ContractService?.Service?.Name,
                    Total = item.InvoiceValue             // contractInvoice.Invoices.Select(x => x.InvoiceValue).Sum()
                });
            }
            var invoiceDetail = new InvoiceDetailDTO
            {
                InvoiceBalanceBeforeReceipting = receipt?.InvoiceValueBalanceBeforeReceipting ?? 0,
                InvoiceDetailsInfos = invoiceDetailsInfo,
                InvoiceDue = contractInvoice.Invoices.FirstOrDefault().DateToBeSent,
                InvoiceEnd = contractInvoice.Invoices.FirstOrDefault().InvoiceEndDate,
                InvoiceStart = contractInvoice.Invoices.FirstOrDefault().InvoiceStartDate,
                InvoiceNumber = contractInvoice.Invoices.FirstOrDefault().InvoiceNumber,
                InvoiceValue = contractInvoice.Invoices.FirstOrDefault().InvoiceValue,
            };

            return invoiceDetail;
        }


        private async Task<ContractInvoiceDTO> GetInvoiceByContractServiceId(long contractServiceId)
        {
            List<Invoice> invoices = new List<Invoice>();
            var contractInvoice = new ContractInvoiceDTO();

            try
            {
                var contractService = await _context.ContractServices.Include(x => x.Contract).Include(x => x.Service)
                   .Where(x => x.Id == contractServiceId && !x.IsDeleted)
                   .FirstOrDefaultAsync();

                if (contractService == null)
                {
                    return null;
                }


                if (String.IsNullOrWhiteSpace(contractService.Contract?.GroupInvoiceNumber))
                {
                    invoices = await _context.Invoices
                    .Include(x => x.Receipts)
                        .Where(x => x.ContractServiceId == contractServiceId
                                    && (bool)x.IsFinalInvoice && x.IsDeleted == false)
                        .OrderBy(x => x.StartDate)
                        .ToListAsync();
                }
                else
                {
                    #region Changes based on new group invoice implementation
                    var groupInvoices = new List<Invoice>();
                    IEnumerable<IGrouping<string, Invoice>> groupedInvoices = null;

                    if (contractService.InvoicingInterval == (int)TimeCycle.Adhoc)
                    {
                        invoices = await _context.Invoices
                            .Include(x => x.ContractService)
                                           .Include(x => x.Receipts)
                                               .Where(x => x.GroupInvoiceNumber == contractService.Contract.GroupInvoiceNumber
                                                                && (bool)x.IsFinalInvoice && !x.IsDeleted)
                                               .OrderBy(x => x.StartDate)
                                               .ToListAsync();

                        groupedInvoices = invoices.GroupBy(x => x.AdhocGroupingId.ToString());
                    }
                    else
                    {
                        invoices = await _context.Invoices
                                                .Include(x => x.Receipts)
                                                .Include(x => x.ContractService)
                                                    .Where(x => x.GroupInvoiceNumber == contractService.Contract.GroupInvoiceNumber
                                                                && (bool)x.IsFinalInvoice && !x.IsDeleted)
                                                    .OrderBy(x => x.StartDate)
                                                    .ToListAsync();

                        foreach (var invoice in invoices)
                        {
                            invoice.StartDate = invoice.StartDate.Date;
                        }

                        groupedInvoices = invoices.GroupBy(x => x.StartDate.ToShortDateString());
                    }

                    foreach (var group in groupedInvoices)
                    {
                        var key = group.Key;

                        double totalAmount = 0;
                        var allReceipts = new List<Receipt>();
                        foreach (var item in group)
                        {
                            totalAmount += item.Value;
                            allReceipts.AddRange(item.Receipts);
                        }

                        var singleInvoice = _mapper.Map<Invoice>(group.FirstOrDefault());

                        singleInvoice.Value = totalAmount;
                        singleInvoice.Receipts = allReceipts;
                        if (group.All(x => x.IsReceiptedStatus == (int)InvoiceStatus.CompletelyReceipted))
                        {
                            singleInvoice.IsReceiptedStatus = (int)InvoiceStatus.CompletelyReceipted;
                        }
                        else if (group.All(x => x.IsReceiptedStatus == (int)InvoiceStatus.NotReceipted))
                        {
                            singleInvoice.IsReceiptedStatus = (int)InvoiceStatus.NotReceipted;
                        }
                        else
                        {
                            singleInvoice.IsReceiptedStatus = (int)InvoiceStatus.PartlyReceipted;
                        }

                        groupInvoices.Add(singleInvoice);
                    }

                    var contractServiceInvoices = new List<ContractServiceInvoiceDTO>();
                    var grpInvoice = new List<InvoiceDTO>();
                    foreach (var item in groupInvoices)
                    {
                        var receiptDetail = item.Receipts.Where(x => x.InvoiceId == item.Id).OrderBy(x => x.Id).FirstOrDefault();
                        var grpInvoiceDetail = item.GroupInvoiceDetails.FirstOrDefault(x => x.ContractServiceId == item.ContractServiceId);
                        var conService = _context.ContractServices.Include(x => x.Service).FirstOrDefault(x => x.Id == item.ContractServiceId);
                        grpInvoice.Add(new InvoiceDTO
                        {
                            InvoiceValueBalanceAfterReceipt = receiptDetail?.InvoiceValueBalanceAfterReceipting,
                            DateToBeSent = item.DateToBeSent,
                            ContractId = (int)item.ContractId,
                            IsFinalInvoice = item.IsFinalInvoice.Value,
                            InvoiceEndDate = item.EndDate,
                            InvoiceNumber = item.InvoiceNumber,
                            InvoiceStartDate = item.StartDate,
                            InvoiceValue = item.Value,
                            IsReceiptedStatus = item.IsReceiptedStatus,
                            Payment = receiptDetail?.ReceiptValue,
                            Id = (int)item.Id,
                            ServiceName = conService.Service.Name
                        });
                        //foreach (var item2 in item.GroupInvoiceDetails)
                        //{
                        //    //grpInvoice.Add(new InvoiceDTO
                        //    //{
                        //    //    InvoiceValueBalanceAfterReceipt = receiptDetail.InvoiceValueBalanceAfterReceipting,
                        //    //    ContractId = (int)item.ContractId,
                        //    //    VAT = grpInvoiceDetail.Vat,
                        //    //    IsReceiptedStatus = item.IsReceiptedStatus,
                        //    //    DateToBeSent = item.DateToBeSent,
                        //    //    InvoiceEndDate = item.EndDate,
                        //    //    InvoiceStartDate = item.StartDate,
                        //    //    InvoiceNumber = item2.InvoiceNumber,
                        //    //    InvoiceValue = receiptDetail.InvoiceValue,
                        //    //    IsFinalInvoice = item.IsFinalInvoice.Value,
                        //    //    Payment = receiptDetail.ReceiptValue,
                        //    //    Id = (int)item.Id
                        //    //});


                        //}


                        contractServiceInvoices.Add(new ContractServiceInvoiceDTO
                        {
                            ContractId = (int)item.ContractId,
                            Invoices = grpInvoice,
                            PaymentsOverDue = grpInvoice.Where(x => x.IsFinalInvoice == true && x.IsReceiptedStatus == 0 && x.InvoiceEndDate < DateTime.Today).Count(),
                            PaymentsDue = grpInvoice.Where(x => x.DateToBeSent < DateTime.Today).Count(),
                            TotalPayments =   grpInvoice.Where(x => x.Payment != null).Select(x => x.Payment.Value).Sum(),
                            Status = grpInvoice.All(x => x.IsReceiptedStatus != 2) ? "Outstanding" : "Completed Payment"
                        });
                    }
                   

                    contractInvoice.ContractServiceInvoices = contractServiceInvoices;

                    return contractInvoice;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("", ex);
            }

            return contractInvoice;
        }

    }
}
