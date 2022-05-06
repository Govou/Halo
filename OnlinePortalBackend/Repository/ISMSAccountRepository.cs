using Halobiz.Common.DTOs.ReceivingDTOs;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Repository
{
    public interface ISMSAccountRepository
    {
        Task<bool> CreateIndividualAccount(SMSIndividualAccountDTO accountDTO);

        Task<bool> CreateBusinessAccount(SMSBusinessAccountDTO accountDTO);
    }
}
