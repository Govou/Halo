﻿using HaloBiz.Model.AccountsModel;
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

        Task<Account> FindAccountByAlias(long alias);

        Task<IEnumerable<Account>> FindAllAccounts();

        Task<Account> UpdateAccount(Account Account);

        Task<bool> DeleteAccount(Account Account);
    }
}
