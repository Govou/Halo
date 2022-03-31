using OnlinePortalBackend.DTOs.TransferDTOs;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Repository
{
    public interface IInvoiceRepository
    {
        Task<ContractServiceInvoiceDTO> GetInvoices(int userId, int? contractService, int? contractId, int limit = 10);
    }
}
