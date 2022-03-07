using System.Collections.Generic;
using System.Threading.Tasks;
using HalobizMigrations.Models;

namespace HaloBiz.Repository
{
    public interface IDeleteLogRepository
    {
        Task<DeleteLog> SaveDeleteLog(DeleteLog deleteLog);
        Task<IEnumerable<DeleteLog>> FindAllDeleteLogs();
    }
}