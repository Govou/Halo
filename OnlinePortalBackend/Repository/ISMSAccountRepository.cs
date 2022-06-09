using Halobiz.Common.DTOs.ReceivingDTOs;
using HalobizMigrations.Models.OnlinePortal;
using OnlinePortalBackend.DTOs.ReceivingDTOs;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Repository
{
    public interface ISMSAccountRepository
    {
        Task<(bool success, string message)> CreateIndividualAccount(SMSIndividualAccountDTO accountDTO);

        Task<(bool success, string message)> CreateBusinessAccount(SMSBusinessAccountDTO accountDTO);

        Task<(bool success, string message)> CreateSupplierAccount(SMSSupplierAccountDTO request);

        Task<OnlineProfile> GetCustomerProfile(int profileId);
    }
}
