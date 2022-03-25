using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Repository
{
    public interface IEndorsementRepository
    {
        Task<Endorsement> FindCartContractById(long userId, long Id);
        Task<IEnumerable<CartContract>> FindAllCartContract(l);
    }
}
