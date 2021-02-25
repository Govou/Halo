using System.Collections.Generic;
using System.Threading.Tasks;
using HaloBiz.Model;

namespace HaloBiz.Repository
{
    public interface IApprovalLimitRepository
    {
        Task<ApprovalLimit> SaveApprovalLimit(ApprovalLimit approvalLimit);
        Task<IEnumerable<ApprovalLimit>> GetApprovalLimits();
        Task<ApprovalLimit> UpdateApprovalLimit(ApprovalLimit approvalLimit);
        Task<bool> DeleteApprovalLimit(ApprovalLimit approvalLimit);
        Task<ApprovalLimit> FindApprovalLimitById(long Id);
    }
}