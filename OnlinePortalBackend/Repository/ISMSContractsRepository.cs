using Halobiz.Common.DTOs.ReceivingDTOs;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Repository
{
    public interface ISMSContractsRepository
    {
        Task<(bool isSuccess, object message)> AddNewContract(SMSContractDTO contractDTO);
        Task<(bool isSuccess, string message)> GenerateInvoiceForContract(SMSCreateInvoiceDTO request);
    }
}
