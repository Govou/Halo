using System.Collections.Generic;
using System.Threading.Tasks;
using HalobizMigrations.Models;

namespace HaloBiz.Repository
{
    public interface IApprovalLimitRepository
    {
        Task<ApprovalLimit> SaveApprovalLimit(ApprovalLimit approvalLimit);
        Task<IEnumerable<ApprovalLimit>> GetApprovalLimits();

        Task<IEnumerable<ApprovalLimit>> GetApprovalLimitsByModule(long moduleId);
        Task<ApprovalLimit> UpdateApprovalLimit(ApprovalLimit approvalLimit);
        Task<bool> DeleteApprovalLimit(ApprovalLimit approvalLimit);
        Task<bool> DeleteApprovalLimitModule(ApprovalLimit approvalLimit);
        Task<ApprovalLimit> FindApprovalLimitById(long Id);
    }
}