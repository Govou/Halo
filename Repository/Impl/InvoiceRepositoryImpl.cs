using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using halobiz_backend.Helpers;
using HalobizMigrations.Data;
using HalobizMigrations.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HaloBiz.Repository.Impl
{
    public class InvoiceRepositoryImpl : IInvoiceRepository
    {
        private readonly HalobizContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<InvoiceRepositoryImpl> _logger;
        public InvoiceRepositoryImpl(HalobizContext context, 
            IMapper mapper,
            ILogger<InvoiceRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
            _mapper = mapper;
        }
        public async Task<Invoice> SaveInvoice(Invoice invoice)
        {
            var invoiceEntity = await _context.Invoices.AddAsync(invoice);
            if(await SaveChanges())
            {
                return invoiceEntity.Entity;
            }
            return null;
        }
        public async Task<IEnumerable<Invoice>> GetInvoice()
        {
            return await _context.Invoices
                .Where(x => x.IsDeleted == false)
                .ToListAsync();
        }

        public async Task<Invoice> UpdateInvoice(Invoice invoice)
        {
            var invoiceEntity =  _context.Invoices.Update(invoice);
            if(await SaveChanges())
            {
                return invoiceEntity.Entity;
            }
            return null;
        }
        public async Task<bool> DeleteInvoice(Invoice invoice)
        {
            invoice.IsDeleted = true;
            _context.Invoices.Update(invoice);
            return await SaveChanges();
        }
        public async Task<Invoice> FindInvoiceById(long Id)
        {
           return await _context.Invoices
           .Include(x => x.CustomerDivision)
            .Include(x => x.Receipts)
            .FirstOrDefaultAsync(x => x.Id == Id && x.IsDeleted == false);
        }

        public async Task<IEnumerable<Invoice>> GetInvoiceByContractServiceId(long contractServiceId) 
        {
            var contractService = await _context.ContractServices
                    .FirstOrDefaultAsync(x => x.Id == contractServiceId && !x.IsDeleted);

            if(contractService == null)
            {
                return new List<Invoice>();
            }

            List<Invoice> invoices;

            if(String.IsNullOrWhiteSpace(contractService.GroupInvoiceNumber))
            {
                invoices = await _context.Invoices
                .Include(x => x.Receipts)
                    .Where(x => x.ContractServiceId == contractServiceId && x.IsDeleted == false)
                    .OrderBy(x => x.StartDate)
                    .ToListAsync();
            }else{
                invoices = await _context.Invoices
                .Include(x => x.Receipts)
                    .Where(x => x.GroupInvoiceNumber == contractService.GroupInvoiceNumber && !x.IsDeleted)
                    .OrderBy(x => x.StartDate)
                    .ToListAsync();
                
                foreach (var invoice in invoices)
                {
                    invoice.GroupInvoiceDetails = await _context.GroupInvoiceDetails
                            .Where(x => x.InvoiceNumber == invoice.GroupInvoiceNumber && !x.IsDeleted).ToListAsync();
                }

                #region Changes based on new group invoice implementation
                var groupInvoices = new List<Invoice>();
                var groupedInvoices = invoices.GroupBy(x => x.StartDate);
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
                    if(group.All(x => x.IsReceiptedStatus == (int)InvoiceStatus.CompletelyReceipted))
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

                invoices = groupInvoices;
                #endregion
            }

            return invoices;
        }

        private async Task<bool> SaveChanges()
        {
           try
           {
               return  await _context.SaveChangesAsync() > 0;
           }
           catch(Exception ex)
           {
               _logger.LogError(ex.Message);
               return false;
           }
        }

        public IQueryable<Invoice> GetInvoiceQueryiable()
        {
            return _context.Invoices.AsQueryable();
        }
    }
}