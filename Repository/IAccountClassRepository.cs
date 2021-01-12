using HaloBiz.Model.AccountsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository
{
    public interface IAccountClassRepository
    {
        Task<AccountClass> SaveAccountClass(AccountClass accountClass);

        Task<AccountClass> FindAccountClassById(long Id);

        Task<AccountClass> FindAccountClassByCaption(string caption);

        Task<IEnumerable<AccountClass>> FindAllAccountClasses();

        Task<AccountClass> UpdateAccountClass(AccountClass accountClass);

        Task<bool> DeleteAccountClass(AccountClass accountClass);
    }
}
