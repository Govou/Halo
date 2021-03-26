using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaloBiz.Data;
using HaloBiz.Model.AccountsModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HaloBiz.Repository.Impl
{
    public class InvoiceRepositoryImpl : IInvoiceRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<InvoiceRepositoryImpl> _logger;
        public InvoiceRepositoryImpl(DataContext context, ILogger<InvoiceRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
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
            var contractServcie = await _context.ContractServices
                    .FirstOrDefaultAsync(x => x.Id == contractServiceId && !x.IsDeleted);

            if(contractServcie == null)
            {
                return new List<Invoice>();
            }

            List<Invoice> invoices;

            if(String.IsNullOrWhiteSpace(contractServcie.GroupInvoiceNumber))
            {
                invoices = await _context.Invoices
                .Include(x => x.Receipts)
                    .Where(x => x.ContractServiceId == contractServiceId && x.IsDeleted == false)
                    .OrderBy(x => x.StartDate)
                    .ToListAsync();
            }else{
                invoices = await _context.Invoices
                .Include(x => x.Receipts)
                    .Where(x => x.GroupInvoiceNumber == contractServcie.GroupInvoiceNumber && !x.IsDeleted)
                    .OrderBy(x => x.StartDate)
                    .ToListAsync();
                
                foreach (var invoice in invoices)
                {
                    invoice.GroupInvoiceDetails = await _context.GroupInvoiceDetails
                            .Where(x => x.InvoiceNumber == invoice.GroupInvoiceNumber && !x.IsDeleted).ToListAsync();
                }

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