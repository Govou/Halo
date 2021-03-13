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
    public class AccountDetailRepositoryImpl : IAccountDetailsRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<AccountDetailRepositoryImpl> _logger;
        public AccountDetailRepositoryImpl(DataContext context, ILogger<AccountDetailRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;

        }

        public async Task<bool> DeleteAccountDetail(AccountDetail accountDetail)
        {
            accountDetail.IsDeleted = true;
            _context.AccountDetails.Update(accountDetail);
            return await SaveChanges();
        }
        public async Task<AccountDetail> FindAccountDetailById(long Id)
        {
            return await _context.AccountDetails.FirstOrDefaultAsync(accountDetail => accountDetail.Id == Id && accountDetail.IsDeleted == false);
        }

        public async Task<IEnumerable<AccountDetail>> FindAllAccountDetails()
        {
            return await _context.AccountDetails.Where(user => user.IsDeleted == false).ToListAsync();

        }
        public async Task<AccountDetail> SaveAccountDetail(AccountDetail accountDetail)
        {
            var AccountDetailEntity = await _context.AccountDetails.AddAsync(accountDetail);
            if (await SaveChanges())
            {
                return AccountDetailEntity.Entity;
            }
            return null;
        }

        public async Task<AccountDetail> UpdateAccountDetail(AccountDetail accountDetail)
        {
            var AccountDetailEntity = _context.AccountDetails.Update(accountDetail);
            if (await SaveChanges())
            {
                return AccountDetailEntity.Entity;
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
