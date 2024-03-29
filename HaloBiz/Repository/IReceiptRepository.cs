using System.Collections.Generic;
using System.Threading.Tasks;
using HalobizMigrations.Models;

namespace HaloBiz.Repository
{
    public interface IReceiptRepository
    {
        Task<Receipt> SaveReceipt(Receipt receipt);
        Task<IEnumerable<Receipt>> GetReceipt();
        Task<Receipt> UpdateReceipt(Receipt receipt);
        Task<bool> DeleteReceipt(Receipt receipt);
        Task<Receipt> FindReceiptById(long Id);
    }
}