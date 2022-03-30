using OnlinePortalBackend.DTOs.TransferDTOs;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Repository
{
    public interface IInvoiceRepository
    {
        Task<ContractServiceInvoiceDTO> GetConractServiceInvoices(int contractServiceId);
    }
}
