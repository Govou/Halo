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

        public async Task<ContractInvoiceDTO>  GetInvoices(long userId)
        {
            var contractInvoiceDTO = new ContractInvoiceDTO();
            var invoices = new List<ContractServiceInvoiceDTO>();
            var finalInvoices = new List<ContractServiceInvoiceDTO>();
            var contractServiceIds = new List<long>();
            // var contractInvoices = _context.Invoices.Where(x => x.CustomerDivisionId == userId).Select(x => x.ContractServiceId).Distinct();
            var contractInvoices = _context.Contracts.Include(x => x.ContractServices).Where(x => x.CustomerDivisionId == userId);
            var contracts = _context.Contracts.Where(x => x.CustomerDivisionId == userId).Select(x => x.Id).ToList();

            foreach (var item in contracts)
            {
                var contractServiceId = _context.ContractServices.Where(x => x.ContractId == item && x.Version == 0).Select(x => x.Id).AsEnumerable();
                contractServiceIds.AddRange(contractServiceId);
            }

            foreach (var item in contractServiceIds)
            {
                var result = await GetInvoiceByContractServiceId(item);
                invoices.AddRange(result.ToList());
            }

            foreach (var item in contracts)
            {
                var cIncoince = invoices.FirstOrDefault(x => x.ContractId == item);
                finalInvoices.Add(cIncoince);
            }
            contractInvoiceDTO.ContractServiceInvoices = finalInvoices;

           return contractInvoiceDTO;
        }

        public async Task<InvoiceDetailDTO> GetInvoice(string invoiceNumber)
        {
            var invoices = _context.Invoices.Include(x => x.Contract).Include(x => x.ContractService).Include(x => x.Receipts).Where(x => x.InvoiceNumber == invoiceNumber);
           // var receipt = _context.Receipts.FirstOrDefault(x => x.)
            var invoiceDetailsInfo = new List<InvoiceDetailInfo>();

            foreach (var item in invoices)
            {
                var invDetail = new InvoiceDetailInfo
                {
                    InvoiceNumber = item.InvoiceNumber,
                    ContractServiceId = (int)item.ContractServiceId,
                    Discount = item.Discount,
                    Quantity = (int)item.Quantity,
                    Total = item.Value
                };
                var conService = _context.ContractServices.Include(x => x.Service).FirstOrDefault(x => x.Id == item.ContractServiceId);
                invDetail.ServiceName = conService.Service.Name;
                invoiceDetailsInfo.Add(invDetail);
            }

            //var contractServiceInvoice = invoices.FirstOrDefault(x => x.ContractId == contractService.ContractId);
            //var grpContractServiceInvoice = contractServiceInvoice.Invoices.Where(x => x.InvoiceStartDate == contractService.StartDate);
            //var contractInvoice = invoices.FirstOrDefault(x => x.ContractId == contractService.ContractId);

            //var receipt = _context.Receipts.Include(x => x.Invoice).Where(x => x.InvoiceId == invoiceId).OrderByDescending(x => x.Id).FirstOrDefault();

           
            var invoiceDetail = new InvoiceDetailDTO
            {
             //   InvoiceBalanceBeforeReceipting = receipt?.InvoiceValueBalanceBeforeReceipting ?? 0,
                InvoiceDetailsInfos = invoiceDetailsInfo,
                InvoiceDue = invoices.FirstOrDefault().EndDate,
                InvoiceEnd = invoices.FirstOrDefault().EndDate,
                InvoiceStart = invoices.FirstOrDefault().StartDate,
                InvoiceNumber = invoices.FirstOrDefault().InvoiceNumber,
                InvoiceValue = invoices.Select(x => x.Value).Sum(),
                
            };

            return invoiceDetail;
        }


        private async Task<IEnumerable<ContractServiceInvoiceDTO>> GetInvoiceByContractServiceId(long contractServiceId)
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

                    var contractServiceInvoices = new List<ContractServiceInvoiceDTO>();
                    var grpInvoice = new List<InvoiceDTO>();
                    foreach (var item in invoices)
                    {
                        var receiptDetail = item.Receipts.Where(x => x.InvoiceId == item.Id).OrderBy(x => x.Id).FirstOrDefault();
                        var grpInvoiceDetail = item.GroupInvoiceDetails.FirstOrDefault(x => x.ContractServiceId == item.ContractServiceId);
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
                            ContractServiceId = (int)contractServiceId
                        });

                        contractServiceInvoices.Add(new ContractServiceInvoiceDTO
                        {
                            ContractId = (int)item.ContractId,
                            Invoices = grpInvoice,
                            PaymentsOverDue = grpInvoice.Where(x => x.IsFinalInvoice == true && x.IsReceiptedStatus == 0 && x.InvoiceEndDate < DateTime.Today).Count(),
                            PaymentsDue = grpInvoice.Where(x => x.DateToBeSent < DateTime.Today).Count(),
                            TotalPayments = grpInvoice.Where(x => x.Payment != null).Select(x => x.Payment.Value).Sum(),
                            Status = grpInvoice.Any(x => x.IsReceiptedStatus != 2) ? "Outstanding" : "Completed Payment"
                        });
                    }



                    return contractInvoice.ContractServiceInvoices = contractServiceInvoices;
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
                            ContractServiceId = (int)contractServiceId
                        });
                        
                        contractServiceInvoices.Add(new ContractServiceInvoiceDTO
                        {
                            ContractId = (int)item.ContractId,
                            Invoices = grpInvoice,
                            PaymentsOverDue = grpInvoice.Where(x => x.IsFinalInvoice == true && x.IsReceiptedStatus == 0 && x.InvoiceEndDate < DateTime.Today).Count(),
                            PaymentsDue = grpInvoice.Where(x => x.DateToBeSent < DateTime.Today).Count(),
                            TotalPayments =   grpInvoice.Where(x => x.Payment != null).Select(x => x.Payment.Value).Sum(),
                            Status = grpInvoice.Any(x => x.IsReceiptedStatus != 2) ? "Outstanding" : "Completed Payment"
                        });
                    }
                   


                    return contractInvoice.ContractServiceInvoices = contractServiceInvoices;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("", ex);
            }

            return contractInvoice?.ContractServiceInvoices;
        }

    }
}
