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
        Task<bool> DeleteApprovingLevelOffice(long approvingLevelOfficeId);
        Task<bool> UpdateApprovingLevelOffice(ApprovingLevelOffice approverLevel);
        Task<bool> RemoveApprovingLevelOfficers(List<ApprovingLevelOfficer> approvingLevelOfficers);
        Task<bool> SaveApprovingLevelOfficers(List<ApprovingLevelOfficer> approvingLevelOfficers);
        Task<ApprovingLevelOffice> FindApprovingLevelOfficeByID(long Id);
        Task<List<ApprovingLevelOfficer>> FindApprovingLevelOfficersByOfficeID(long Id);
    }
}