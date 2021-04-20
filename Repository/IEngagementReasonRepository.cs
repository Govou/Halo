using System.Collections.Generic;
using System.Threading.Tasks;
using HaloBiz.Model;

namespace HaloBiz.Repository
{
    public interface IEngagementReasonRepository
    {
        Task<EngagementReason> SaveEngagementReason(EngagementReason engagementReason);
        Task<IEnumerable<EngagementReason>> GetEngagementReasons();
        Task<EngagementReason> UpdateEngagementReason(EngagementReason engagementReason);
        Task<bool> DeleteEngagementReason(EngagementReason engagementReason);
        Task<EngagementReason> FindEngagementReasonById(long Id);
    }
}