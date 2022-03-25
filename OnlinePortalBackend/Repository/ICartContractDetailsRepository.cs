using HalobizMigrations.Models.OnlinePortal;
using OnlinePortalBackend.DTOs.TransferDTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Repository
{
    public interface ICartContractDetailsRepository
    {
        Task<CartContractService> SaveCartContractService(CartContractService entity);
        Task<CartContractService> FindCartContractServiceById(long Id);
        Task<IEnumerable<CartContractService>> FindAllCartContractServicesForAContract(long contractId);
        Task<IEnumerable<CartContractService>> FindCartContractServicesByReferenceNumber(string refNo);
        Task<CartContractService> FindCartContractServiceByTag(string tag);
        Task<IEnumerable<CartContractService>> FindCartContractServicesByGroupInvoiceNumber(string groupInvoiceNumber);
        Task<CartContractService> UpdateCartContractService(CartContractService entity);
        Task<bool> DeleteCartContractService(CartContractService entity);

    }
}
