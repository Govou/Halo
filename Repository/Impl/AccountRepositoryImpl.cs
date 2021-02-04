﻿using HaloBiz.Data;
using HaloBiz.Model.AccountsModel;
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
        private readonly DataContext _context;
        private readonly ILogger<AccountRepositoryImpl> _logger;
        public AccountRepositoryImpl(DataContext context, ILogger<AccountRepositoryImpl> logger)
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
            return await _context.Accounts.FirstOrDefaultAsync(Account => Account.Id == Id && Account.IsDeleted == false);
        }

        public async Task<IEnumerable<Account>> FindAllAccounts()
        {
            return await _context.Accounts.Where(user => user.IsDeleted == false).ToListAsync();

        }

        public async Task<Account> SaveAccount(Account account)
        {
            await _context.Database.OpenConnectionAsync();
            using(var transaction = await _context.Database.BeginTransactionAsync()){
            try{
                await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.Accounts ON");

                var  lastSavedAccount = await _context.Accounts.Where(x => x.ControlAccountId == account.ControlAccountId)
                    .OrderBy(x => x.Id).LastOrDefaultAsync();
                if(lastSavedAccount == null || lastSavedAccount.Id < 1000000000)
                {
                    account.Id = (long) account.ControlAccountId + 1;
                }else{
                    account.Id = lastSavedAccount.Id + 1;
                }
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
                
                await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.Accounts OFF");
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
