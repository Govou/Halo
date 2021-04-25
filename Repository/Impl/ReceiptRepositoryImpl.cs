using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HalobizMigrations.Data;
using halobiz_backend.DTOs.ReceivingDTOs;
using HalobizMigrations.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HaloBiz.Repository.Impl
{
    public class ReceiptRepositoryImpl : IReceiptRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<ReceiptRepositoryImpl> _logger;

        public ReceiptRepositoryImpl(HalobizContext context, ILogger<ReceiptRepositoryImpl> logger)
        {
            this._context = context;
            this._logger = logger;
        }

        public async Task<Receipt> SaveReceipt(Receipt receipt)
        {
            var receiptEntity = await _context.Receipts.AddAsync(receipt);
            if(await SaveChanges())
            {
                return receiptEntity.Entity;
            }
            return null;
        }
        public async Task<IEnumerable<Receipt>> GetReceipt()
        {
            return await _context.Receipts
                .Where(x => x.IsDeleted == false)
                .ToListAsync();
        }

        public async Task<Receipt> UpdateReceipt(Receipt receipt)
        {
            var receiptEntity =  _context.Receipts.Update(receipt);
            if(await SaveChanges())
            {
                return receiptEntity.Entity;
            }
            return null;
        }
        public async Task<bool> DeleteReceipt(Receipt receipt)
        {
            receipt.IsDeleted = true;
            _context.Receipts.Update(receipt);
            return await SaveChanges();
        }
        public async Task<Receipt> FindReceiptById(long Id)
        {
           return await _context.Receipts
            .FirstOrDefaultAsync(x => x.Id == Id && x.IsDeleted == false);
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

    }
}