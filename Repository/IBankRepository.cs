using System.Collections.Generic;
using System.Threading.Tasks;
using HaloBiz.Model;

namespace HaloBiz.Repository
{
    public interface IBankRepository
    {
        Task<Bank> SaveBank(Bank bank);
        Task<Bank> FindBankById(long Id);
        Task<Bank> FindBankByName(string name);
        Task<IEnumerable<Bank>> FindAllBank();
        Task<Bank> UpdateBank(Bank bank);
        Task<bool> DeleteBank(Bank bank);
    }
}