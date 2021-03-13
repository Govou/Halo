using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaloBiz.Model.AccountsModel;

namespace HaloBiz.Repository
{
    public interface IControlAccountRepository
    {
        Task<ControlAccount> SaveControlAccount(ControlAccount controlAccount);
        Task<ControlAccount> FindControlAccountById(long Id);
        Task<ControlAccount> FindControlAccountByAlias(string alias);
        Task<ControlAccount> FindControlAccountByName(string name);
        Task<IEnumerable<ControlAccount>> FindAllControlAccount();
        IQueryable<ControlAccount> GetControlAccountQueryable();
        Task<ControlAccount> UpdateControlAccount(ControlAccount controlAccount);
        Task<bool> DeleteControlAccount(ControlAccount controlAccount);
    }
}