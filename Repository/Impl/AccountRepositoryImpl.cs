using HalobizMigrations.Data;
using HalobizMigrations.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository.Impl
{
    public class AccountRepositoryImpl : IAccountRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<AccountRepositoryImpl> _logger;
        private readonly string SALES_INCOME_CONTROL = "Trade Income";
        private readonly string CASH_BOOK = "CASH BOOK";
        public AccountRepositoryImpl(HalobizContext context, ILogger<AccountRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<bool> DeleteAccount(Account Account)
        {
            Account.IsDeleted = true;
            _context.Accounts.Update(Account);
            return await SaveChanges();
        }

        public async Task<Account> FindAccountByAlias(string alias)
        {
            return await _context.Accounts.FirstOrDefaultAsync(account => account.Alias == alias && account.IsDeleted == false);
        }

        public async Task<Account> FindAccountById(long Id)
        {
            return await _context.Accounts.AsNoTracking()
                .Include(x => x.AccountDetails)
                .FirstOrDefaultAsync(Account => Account.Id == Id && Account.IsDeleted == false);
        }

        public async Task<IEnumerable<Account>> FindAllAccounts()
        {
            return await _context.Accounts
                    .Where(user => user.IsDeleted == false).ToListAsync();

        }
        public async Task<IEnumerable<Account>> GetCashAccounts()
        {
            var controlAccount = await _context.ControlAccounts.FirstOrDefaultAsync(x => 
                     x.Caption.ToUpper().Trim() == this.CASH_BOOK);
            if(controlAccount == null)
            {
                return new List<Account>();
            }
            return await _context.Accounts
                    .Where(x => x.IsDeleted == false && x.ControlAccountId == controlAccount.Id).ToListAsync();
        }

        public  IQueryable<Account> GetAccountQueriable()
        {
            return  _context.Accounts.Include(x => x.AccountDetails).AsQueryable();
        }
        public async Task<IEnumerable<Account>> FindAllTradeIncomeAccounts()
        {
            var  controlAccount = await _context.ControlAccounts
                .Include(x => x.Accounts)
                .FirstOrDefaultAsync(x => x.Caption == SALES_INCOME_CONTROL);
            return controlAccount.Accounts;
        }

        public async Task<Account> SaveAccount(Account account)
        {
            await _context.Database.OpenConnectionAsync();
            using(var transaction = await _context.Database.BeginTransactionAsync()){
            try{
              //  await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.Accounts ON");

                var  lastSavedAccount = await _context.Accounts.Where(x => x.ControlAccountId == account.ControlAccountId)
                    .OrderBy(x => x.Id).LastOrDefaultAsync();
                    var _controlAccount = await _context.ControlAccounts.Where(x => x.Id == account.ControlAccountId).FirstOrDefaultAsync();
                if(lastSavedAccount == null || lastSavedAccount.AccountNumber < 1000000000)
                {
                    account.AccountNumber = _controlAccount.AccountNumber + 1;
                }else{
                    account.AccountNumber = lastSavedAccount.AccountNumber + 1;
                }
                var controlAccount = await _context.ControlAccounts
                        .Include(x => x.AccountClass).FirstOrDefaultAsync(x => x.Id == account.ControlAccountId);
                account.IsDebitBalance = controlAccount.AccountClass.Caption.ToLower().Contains("asset") || controlAccount.AccountClass.Caption.ToLower().Contains("expense");
                var savedAccount = await _context.Accounts.AddAsync(account);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return savedAccount.Entity;
            }catch(Exception e)
            {
                await transaction.RollbackAsync();
                _logger.LogError(e.Message);
                _logger.LogError(e.StackTrace);
                return null;
            }finally{
                
              //  await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.Accounts OFF");
                await _context.Database.CloseConnectionAsync();
            }
            }
           
        }

        public async Task<Account> UpdateAccount(Account Account)
        {
            var AccountEntity = _context.Accounts.Update(Account);
            if (await SaveChanges())
            {
                return AccountEntity.Entity;
            }
            return null;
        }
        private async Task<bool> SaveChanges()
        {
            try
            {
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }
    }
}
