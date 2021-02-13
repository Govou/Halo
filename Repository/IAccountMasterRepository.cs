using HaloBiz.Model.AccountsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository
{
    public interface IAccountMasterRepository
    {
        Task<AccountMaster> SaveAccountMaster(AccountMaster accountMaster);

        Task<AccountMaster> FindAccountMasterById(long Id);

        Task<IEnumerable<AccountMaster>> FindAllAccountMasters();

        Task<AccountMaster> UpdateAccountMaster(AccountMaster accountMaster);

        Task<bool> DeleteAccountMaster(AccountMaster accountMaster);
    }
}
