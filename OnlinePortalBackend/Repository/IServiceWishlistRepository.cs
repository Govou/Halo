using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HalobizMigrations.Models.OnlinePortal;

namespace OnlinePortalBackend.Repository
{
    public interface IServiceWishlistRepository
    {
        Task<ServiceWishlist> SaveServiceWishlist(ServiceWishlist serviceWishlist);
        Task<bool> UpdateServiceWishlists(IEnumerable<ServiceWishlist> serviceWishlist);
        Task<ServiceWishlist> FindServiceWishlistById(long Id);
        Task<IEnumerable<ServiceWishlist>> FindServiceWishlistsByProspectId(long prospectId);
        Task<IEnumerable<ServiceWishlist>> FindAllServiceWishlists();
        Task<ServiceWishlist> UpdateServiceWishlist(ServiceWishlist serviceWishlist);
        Task<bool> RemoveServiceWishlist(ServiceWishlist serviceWishlist);
    }
}