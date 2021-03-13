using System.Collections.Generic;
using System.Threading.Tasks;
using HaloBiz.Model;

namespace HaloBiz.Repository
{
    public interface IDeleteLogRepository
    {
        Task<DeleteLog> SaveDeleteLog(DeleteLog deleteLog);
        Task<IEnumerable<DeleteLog>> FindAllDeleteLogs();
    }
}