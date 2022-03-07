using System.Collections.Generic;
using System.Threading.Tasks;
using HalobizMigrations.Models;

namespace HaloBiz.Repository.LAMS
{
    public interface IDropReasonRepository
    {
        Task<DropReason> SaveDropReason(DropReason DropReason);
        Task<DropReason> FindDropReasonById(long Id);
        Task<DropReason> FindDropReasonByTitle(string title);
        Task<IEnumerable<DropReason>> FindAllDropReason();
        Task<DropReason> UpdateDropReason(DropReason DropReason);
        Task<bool> DeleteDropReason(DropReason DropReason);
    }
}