using System.Collections.Generic;
using System.Threading.Tasks;
using HaloBiz.Model;

namespace HaloBiz.Repository
{
    public interface IApprovalRepository
    {
        Task<Approval> SaveApproval(Approval approval);
        Task<IEnumerable<Approval>> GetApprovals();
        Task<Approval> UpdateApproval(Approval approval);
        Task<bool> DeleteApproval(Approval approval);
        Task<Approval> FindApprovalById(long Id);
        Task<IEnumerable<Approval>> GetPendingApprovals();
        Task<IEnumerable<Approval>> GetUserPendingApprovals(long userId);
        Task<bool> SaveApprovalRange(List<Approval> approvals);
        Task<IEnumerable<Approval>> GetPendingApprovalsByQuoteId(long quoteId);
        Task<IEnumerable<Approval>> GetApprovalsByQuoteId(long quoteId);
        Task<IEnumerable<Approval>> GetPendingApprovalsByServiceId(long serviceId);
    }
}