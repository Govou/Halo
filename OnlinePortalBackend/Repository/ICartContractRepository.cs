using HalobizMigrations.Models.OnlinePortal;
using Microsoft.AspNetCore.Http;
using OnlinePortalBackend.DTOs.TransferDTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Repository
{
    public interface ICartContractRepository
    {
        Task<CartContract> SaveCartContract(long userId, CartContractDTO cartContractDTO);
        Task<CartContract> FindCartContractById(long userId, long Id);
        Task<IEnumerable<CartContract>> FindAllCartContract();
        Task<CartContract> FindCartContractByReferenceNumber(string refNo);
        Task<CartContract> UpdateCartContract(CartContract entity);
        Task<bool> DeleteCartContract(CartContract entity);
    }
}
