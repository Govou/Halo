using System.Collections.Generic;
using System.Threading.Tasks;
using HaloBiz.Model.LAMS;

namespace HaloBiz.Repository.LAMS
{
    public interface ILeadEngagementRepository
    {
        Task<LeadEngagement> SaveLeadEngagement(LeadEngagement leadEngagement);
        Task<LeadEngagement> FindLeadEngagementById(long Id);
        Task<LeadEngagement> FindLeadEngagementByName(string name);
        Task<IEnumerable<LeadEngagement>> FindAllLeadEngagement();
        Task<LeadEngagement> UpdateLeadEngagement(LeadEngagement leadEngagement);
        Task<bool> DeleteLeadEngagement(LeadEngagement leadEngagement);
    }
}