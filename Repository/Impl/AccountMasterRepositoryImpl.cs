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
    public class AccountMasterRepositoryImpl : IAccountMasterRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<AccountMasterRepositoryImpl> _logger;
        public AccountMasterRepositoryImpl(DataContext context, ILogger<AccountMasterRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;

        }

        public async Task<bool> DeleteAccountMaster(AccountMaster accountMaster)
        {
            accountMaster.IsDeleted = true;
            _context.AccountMasters.Update(accountMaster);
            return await SaveChanges();
        }

        public async Task<AccountMaster> FindAccountMasterById(long Id)
        {
            return await _context.AccountMasters.FirstOrDefaultAsync(accountMaster => accountMaster.Id == Id && accountMaster.IsDeleted == false);
        }

        public async Task<IEnumerable<AccountMaster>> FindAllAccountMasters()
        {
            return await _context.AccountMasters.Where(user => user.IsDeleted == false).ToListAsync();

        }
        public async Task<AccountMaster> SaveAccountMaster(AccountMaster accountMaster)
        {
            var AccountMasterEntity = await _context.AccountMasters.AddAsync(accountMaster);
            if (await SaveChanges())
            {
                return AccountMasterEntity.Entity;
            }
            return null;
        }

        public async Task<AccountMaster> UpdateAccountMaster(AccountMaster accountMaster)
        {
            var AccountMasterEntity = _context.AccountMasters.Update(accountMaster);
            if (await SaveChanges())
            {
                return AccountMasterEntity.Entity;
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
