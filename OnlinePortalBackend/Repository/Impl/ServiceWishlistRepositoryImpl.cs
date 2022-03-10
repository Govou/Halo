using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HalobizMigrations.Models.OnlinePortal;
using HalobizMigrations.Data;

namespace OnlinePortalBackend.Repository.Impl
{
    public class ServiceWishlistRepositoryImpl : IServiceWishlistRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<ServiceWishlistRepositoryImpl> _logger;
        public ServiceWishlistRepositoryImpl(HalobizContext context, ILogger<ServiceWishlistRepositoryImpl> logger)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<ServiceWishlist> SaveServiceWishlist(ServiceWishlist serviceWishlist)
        {
            var serviceWishlistEntity = await _context.ServiceWishlists.AddAsync(serviceWishlist);
            if (await SaveChanges())
            {
                return serviceWishlistEntity.Entity;
            }
            return null;
        }

        public async Task<bool> UpdateServiceWishlists(IEnumerable<ServiceWishlist> serviceWishlists)
        {
            _context.ServiceWishlists.UpdateRange(serviceWishlists);
            return await SaveChanges();
        }

        public async Task<ServiceWishlist> FindServiceWishlistById(long Id)
        {
            return await _context.ServiceWishlists
                .FirstOrDefaultAsync(user => user.Id == Id && user.IsDeleted == false);
        }
        
        public async Task<IEnumerable<ServiceWishlist>> FindServiceWishlistsByProspectId(long prospectId)
        {
            return await _context.ServiceWishlists
                            .Where(x => x.ProspectId == prospectId && x.IsDeleted == false)
                            .ToListAsync();
        }

        public async Task<IEnumerable<ServiceWishlist>> FindAllServiceWishlists()
        {
            return await _context.ServiceWishlists
                .Where(user => user.IsDeleted == false)
                .ToListAsync();
        }

        public async Task<ServiceWishlist> UpdateServiceWishlist(ServiceWishlist serviceWishlist)
        {
            var ServiceWishlistEntity = _context.ServiceWishlists.Update(serviceWishlist);
            if (await SaveChanges())
            {
                return ServiceWishlistEntity.Entity;
            }
            return null;
        }

        public async Task<bool> RemoveServiceWishlist(ServiceWishlist serviceWishlist)
        {
            serviceWishlist.IsDeleted = true;
            _context.ServiceWishlists.Update(serviceWishlist);
            return await SaveChanges();
        }

        private async Task<bool> SaveChanges()
        {
            try{
                return await _context.SaveChangesAsync() > 0;
            }catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }
    }
}