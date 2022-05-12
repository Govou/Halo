using Halobiz.Common.DTOs.ReceivingDTOs;
using HalobizMigrations.Models.OnlinePortal;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Repository
{
    public interface ISMSAccountRepository
    {
        Task<bool> CreateIndividualAccount(SMSIndividualAccountDTO accountDTO);

        Task<bool> CreateBusinessAccount(SMSBusinessAccountDTO accountDTO);

        Task<OnlineProfile> GetCustomerProfile(int profileId);
    }
}
