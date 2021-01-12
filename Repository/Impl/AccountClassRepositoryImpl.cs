using HaloBiz.Data;
using HaloBiz.Model.AccountsModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository.Impl
{
    public class AccountClassRepositoryImpl : IAccountClassRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<AccountClassRepositoryImpl> _logger;
        public AccountClassRepositoryImpl(DataContext context, ILogger<AccountClassRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;

        }

        public async Task<bool> DeleteAccountClass(AccountClass accountClass)
        {
            accountClass.IsDeleted = true;
            _context.AccountClasses.Update(accountClass);
            return await SaveChanges();
        }

        public async Task<AccountClass> FindAccountClassByCaption(string caption)
        {
            return await _context.AccountClasses.FirstOrDefaultAsync(accountClass => accountClass.Caption == caption && accountClass.IsDeleted == false);

        }

        public async Task<AccountClass> FindAccountClassById(long Id)
        {
            return await _context.AccountClasses.FirstOrDefaultAsync(accountClass => accountClass.Id == Id && accountClass.IsDeleted == false);
        }

        public async Task<IEnumerable<AccountClass>> FindAllAccountClasses()
        {
            return await _context.AccountClasses.Where(user => user.IsDeleted == false).ToListAsync();

        }

        public async Task<AccountClass> SaveAccountClass(AccountClass accountClass)
        {
            var AccountClassEntity = await _context.AccountClasses.AddAsync(accountClass);
            if (await SaveChanges())
            {
                return AccountClassEntity.Entity;
            }
            return null;
        }

        public async Task<AccountClass> UpdateAccountClass(AccountClass accountClass)
        {
            var AccountClassEntity = _context.AccountClasses.Update(accountClass);
            if (await SaveChanges())
            {
                return AccountClassEntity.Entity;
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
