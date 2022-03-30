using HalobizMigrations.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Repository
{
    public interface IEndorsementRepository
    {
        Task<ContractServiceForEndorsement> FindEndorsementById(long userId, long Id);
        Task<IEnumerable<ContractServiceForEndorsement>> FindEndorsements(long userId, int limit);
    }
}
