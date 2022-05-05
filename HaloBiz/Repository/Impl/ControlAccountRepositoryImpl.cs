using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HalobizMigrations.Data;
using HalobizMigrations.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HaloBiz.Repository.Impl
{
    public class ControlAccountRepositoryImpl : IControlAccountRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<ControlAccountRepositoryImpl> _logger;
        public ControlAccountRepositoryImpl(HalobizContext context, ILogger<ControlAccountRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<ControlAccount> SaveControlAccount(ControlAccount controlAccount)
        {
            
            using(var transaction = await _context.Database.BeginTransactionAsync())
            {
                try{
                    await _context.Database.OpenConnectionAsync();

                    var  lastSavedControl = await _context.ControlAccounts.Where(control => control.AccountClassId == controlAccount.AccountClassId)
                        .OrderBy(control => control.CreatedAt).LastOrDefaultAsync();

                    
                    if(lastSavedControl == null || lastSavedControl.AccountNumber < 100000000)
                    {
                        var accountClass = await _context.AccountClasses.Where(x => x.Id == controlAccount.AccountClassId).FirstOrDefaultAsync();
                        var num = accountClass.AccountNumber.ToString(); // = controlAccount.AccountNumber + 100000000;
                        var nums = num.ToCharArray();
                        int counter = 0;
                        string account = string.Empty;
                        foreach (var item in num)
                        {
                            if (counter == 1)
                            {
                                account += 1;
                            }
                            else
                            {
                                account += item;
                            }

                            ++counter;
                        }
                        controlAccount.AccountNumber = long.Parse(account);
                    }else{
                        var num = lastSavedControl.AccountNumber;
                        var firstCharacter = num.ToString()[0];
                        var otherNumbers = num.ToString()[1..].Replace("0", "");
                        string id;
                        if(otherNumbers.EndsWith("9"))
                        {
                            id = firstCharacter + (int.Parse(otherNumbers) + 2).ToString().PadRight(9, '0');
                        }
                        else
                        {
                            id = firstCharacter + (int.Parse(otherNumbers) + 1).ToString().PadRight(9, '0');
                        }

                        controlAccount.AccountNumber = long.Parse(id);
                    }
                    var savedControlAccount = await _context.ControlAccounts.AddAsync(controlAccount);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return savedControlAccount.Entity;
                }catch(Exception e)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(e.Message);
                    _logger.LogError(e.StackTrace);
                    return null;
                }finally{
                    await _context.Database.CloseConnectionAsync();
                }
            }

        }

        public async Task<ControlAccount> FindControlAccountById(long Id)
        {
            return await _context.ControlAccounts
                .Include(control => control.Accounts.Where(x => x.IsDeleted == false))
                .FirstOrDefaultAsync( controlAccount => controlAccount.Id == Id && controlAccount.IsDeleted == false);
        }

        public async Task<ControlAccount> FindControlAccountByName(string name)
        {
            return await _context.ControlAccounts
                .Include(control => control.Accounts.Where(x => x.IsDeleted == false))
                .FirstOrDefaultAsync( controlAccount => controlAccount.Caption == name && controlAccount.IsDeleted == false);
        }
        public async Task<ControlAccount> FindControlAccountByAlias(string name)
        {
            return await _context.ControlAccounts
                .Include(control => control.Accounts.Where(x => x.IsDeleted == false))
                .FirstOrDefaultAsync( controlAccount => controlAccount.Alias== name && controlAccount.IsDeleted == false);
        }

        public async Task<IEnumerable<ControlAccount>> FindAllControlAccount()
        {
            return await _context.ControlAccounts
                .Include(controlAccount => controlAccount.Accounts)
                .Where(controlAccount => controlAccount.IsDeleted == false)
                .OrderByDescending(controlAccount => controlAccount.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<ControlAccount>> FindAllIncomeControlAccount()
        {
            return await _context.ControlAccounts
                .Where(controlAccount => controlAccount.AccountClass.Caption.ToLower() == "revenue" 
                        && !controlAccount.IsDeleted)
                .OrderByDescending(controlAccount => controlAccount.CreatedAt)
                .ToListAsync();
        }

        public async Task<ControlAccount> UpdateControlAccount(ControlAccount controlAccount)
        {
            var controlAccountEntity =  _context.ControlAccounts.Update(controlAccount);
            if(await SaveChanges())
            {
                return controlAccountEntity.Entity;
            }
            return null;
        }

        public async Task<bool> DeleteControlAccount(ControlAccount controlAccount)
        {
            controlAccount.IsDeleted = true;
            _context.ControlAccounts.Update(controlAccount);
            return await SaveChanges();
        }
        public IQueryable<ControlAccount> GetControlAccountQueryable()
        {
            return _context.ControlAccounts.AsQueryable();
        }
        private async Task<bool> SaveChanges()
        {
           try{
               return  await _context.SaveChangesAsync() > 0;
           }catch(Exception ex)
           {
               _logger.LogError(ex.Message);
               return false;
           }
        }
    }
}