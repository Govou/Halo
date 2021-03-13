using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaloBiz.Data;
using HaloBiz.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HaloBiz.Repository.Impl
{
    public class BankRepositoryImpl : IBankRepository
    {
         private readonly DataContext _context;
        private readonly ILogger<BankRepositoryImpl> _logger;
        public BankRepositoryImpl(DataContext context, ILogger<BankRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<Bank> SaveBank(Bank bank)
        {
            var bankEntity = await _context.Banks.AddAsync(bank);
            if(await SaveChanges())
            {
                return bankEntity.Entity;
            }
            return null;
        }

        public async Task<Bank> FindBankById(long Id)
        {
            return await _context.Banks
                .Where(Bank => Bank.IsDeleted == false)
                .FirstOrDefaultAsync( bank => bank.Id == Id && bank.IsDeleted == false);
        }

        public async Task<Bank> FindBankByName(string name)
        {
            return await _context.Banks
                .FirstOrDefaultAsync( bank => bank.Name == name && bank.IsDeleted == false);
        }

        public async Task<IEnumerable<Bank>> FindAllBank()
        {
            return await _context.Banks
                .Where(bank => bank.IsDeleted == false)
                .OrderBy(bank => bank.CreatedAt)
                .ToListAsync();
        }

        public async Task<Bank> UpdateBank(Bank bank)
        {
            var bankEntity =  _context.Banks.Update(bank);
            if(await SaveChanges())
            {
                return bankEntity.Entity;
            }
            return null;
        }

        public async Task<bool> DeleteBank(Bank bank)
        {
            bank.IsDeleted = true;
            _context.Banks.Update(bank);
            return await SaveChanges();
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