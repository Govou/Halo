using System.Collections.Generic;
using System.Threading.Tasks;
using HalobizMigrations.Models;

namespace HaloBiz.Repository
{
    public interface IModificationHistoryRepository
    {
        Task<ModificationHistory> SaveHistory(ModificationHistory history);
        Task<ModificationHistory> FindHistoryByEntityChanged(string entityName);
        Task<ModificationHistory> FindHistoryByUserName(string firstName);
        Task<IEnumerable<ModificationHistory>> FindAllModifications();
        Task<bool> RemoveModificationRecord(ModificationHistory record);
    }
}