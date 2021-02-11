using HaloBiz.Model.AccountsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository
{
    public interface IAccountDetailsRepository
    {
        Task<AccountDetail> SaveAccountDetail(AccountDetail accountDetail);

        Task<AccountDetail> FindAccountDetailById(long Id);

        Task<IEnumerable<AccountDetail>> FindAllAccountDetails();

        Task<AccountDetail> UpdateAccountDetail(AccountDetail accountDetail);

        Task<bool> DeleteAccountDetail(AccountDetail accountDetail);
    }
}
