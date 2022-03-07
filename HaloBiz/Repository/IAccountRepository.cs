using HalobizMigrations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository
{
    public interface IAccountRepository
    {
        Task<Account> SaveAccount(Account Account);

        Task<Account> FindAccountById(long Id);

        Task<Account> FindAccountByAlias(string alias);

        Task<IEnumerable<Account>> FindAllAccounts();

        Task<Account> UpdateAccount(Account Account);

        Task<bool> DeleteAccount(Account Account);
        Task<IEnumerable<Account>> FindAllTradeIncomeAccounts();
         IQueryable<Account> GetAccountQueriable();
          Task<IEnumerable<Account>> GetCashAccounts();
    }
}
