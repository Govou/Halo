using System.Collections.Generic;
using System.Threading.Tasks;
using HalobizMigrations.Models;

namespace HaloBiz.Repository
{
    public interface IApproverLevelRepository
    {
        Task<ApproverLevel> SaveApproverLevel(ApproverLevel approverLevel);
        Task<IEnumerable<ApproverLevel>> GetApproverLevels();
        Task<ApproverLevel> UpdateApproverLevel(ApproverLevel approverLevel);
        Task<bool> DeleteApproverLevel(ApproverLevel approverLevel);
        Task<ApproverLevel> FindApproverLevelById(long Id);
        Task<ApprovingLevelOffice> GetLastApprovingLevelOffice();
        Task<bool> SaveApprovingLevelOffice(ApprovingLevelOffice approvingLevelOffice);
        Task<IEnumerable<ApprovingLevelOffice>> GetApprovingLevelOffices();
    }
}