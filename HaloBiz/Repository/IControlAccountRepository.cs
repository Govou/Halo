using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HalobizMigrations.Models;

namespace HaloBiz.Repository
{
    public interface IControlAccountRepository
    {
        Task<ControlAccount> SaveControlAccount(ControlAccount controlAccount);
        Task<ControlAccount> FindControlAccountById(long Id);
        Task<ControlAccount> FindControlAccountByAlias(string alias);
        Task<ControlAccount> FindControlAccountByName(string name);
        Task<IEnumerable<ControlAccount>> FindAllControlAccount();
        Task<IEnumerable<ControlAccount>> FindAllIncomeControlAccount();
        IQueryable<ControlAccount> GetControlAccountQueryable();
        Task<ControlAccount> UpdateControlAccount(ControlAccount controlAccount);
        Task<bool> DeleteControlAccount(ControlAccount controlAccount);
    }
}