using System.Collections.Generic;
using System.Threading.Tasks;
using HalobizMigrations.Models;

namespace HaloBiz.Repository.LAMS
{
    public interface IClientEngagementRepository
    {
        Task<ClientEngagement> SaveClientEngagement(ClientEngagement clientEngagement);
        Task<ClientEngagement> FindClientEngagementById(long Id);
        Task<ClientEngagement> FindClientEngagementByName(string name);
        Task<IEnumerable<ClientEngagement>> FindAllClientEngagement();
        Task<ClientEngagement> UpdateClientEngagement(ClientEngagement clientEngagement);
        Task<bool> DeleteClientEngagement(ClientEngagement clientEngagement);
    }
}