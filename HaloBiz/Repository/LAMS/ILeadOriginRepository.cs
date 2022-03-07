using HalobizMigrations.Models;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace HaloBiz.Repository.LAMS
{
    public interface ILeadOriginRepository
    {
         Task<LeadOrigin> SaveLeadOrigin(LeadOrigin leadOrigin);
         Task<LeadOrigin> FindLeadOriginById(long Id);
         Task<LeadOrigin> FindLeadOriginByName(string name);
        Task<IEnumerable<LeadOrigin>> FindAllLeadOrigin();
         Task<LeadOrigin> UpdateLeadOrigin(LeadOrigin leadOrigin);
         Task<bool> DeleteLeadOrigin(LeadOrigin leadOrigin);
    }
}