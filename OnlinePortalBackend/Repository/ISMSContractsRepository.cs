using Halobiz.Common.DTOs.ReceivingDTOs;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Repository
{
    public interface ISMSContractsRepository
    {
        Task<bool> AddNewContract(SMSContractDTO contractDTO);
    }
}
