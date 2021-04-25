using System.Collections.Generic;
using System.Threading.Tasks;
using HalobizMigrations.Models;

namespace HaloBiz.Repository
{
    public interface IEngagementTypeRepository
    {
        Task<EngagementType> SaveEngagementType(EngagementType engagementType);
        Task<IEnumerable<EngagementType>> GetEngagementTypes();
        Task<EngagementType> UpdateEngagementType(EngagementType engagementType);
        Task<bool> DeleteEngagementType(EngagementType engagementType);
        Task<EngagementType> FindEngagementTypeById(long Id);
    }
}