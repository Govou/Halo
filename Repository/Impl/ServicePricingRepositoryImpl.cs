using HalobizMigrations.Data;
using HalobizMigrations.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository.Impl
{
    public class ServicePricingRepositoryImpl : IServicePricingRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<ServicePricingRepositoryImpl> _logger;
        public ServicePricingRepositoryImpl(HalobizContext context, ILogger<ServicePricingRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<bool> DeleteServicePricing(ServicePricing serviceType)
        {
            serviceType.IsDeleted = true;
            _context.ServicePricings.Update(serviceType);
            return await SaveChanges();
        }

        public async Task<IEnumerable<ServicePricing>> FindAllServicePricings()
        {
            return await _context.ServicePricings
               .Include(x => x.Service)
               .Include(x => x.Branch)
               .Where(serviceType => serviceType.IsDeleted == false)
               .OrderBy(serviceType => serviceType.CreatedAt)
               .ToListAsync();
        }

        public async Task<ServicePricing> FindServicePricingById(long Id)
        {
            return await _context.ServicePricings
                .Where(serviceType => serviceType.IsDeleted == false)
                .FirstOrDefaultAsync(serviceType => serviceType.Id == Id && serviceType.IsDeleted == false);

        }

        /*public async Task<ServicePricing> FindServicePricingByName(string name)
        {
            return await _context.ServicePricings
                 .Where(serviceType => serviceType.IsDeleted == false)
                 .FirstOrDefaultAsync(serviceType => serviceType.Caption == name && serviceType.IsDeleted == false);

        }*/

        public async Task<ServicePricing> SaveServicePricing(ServicePricing serviceType)
        {
            var serviceTypeEntity = await _context.ServicePricings.AddAsync(serviceType);
            if (await SaveChanges())
            {
                return serviceTypeEntity.Entity;
            }
            return null;
        }

        public async Task<ServicePricing> UpdateServicePricing(ServicePricing serviceType)
        {
            var serviceTypeEntity = _context.ServicePricings.Update(serviceType);
            if (await SaveChanges())
            {
                return serviceTypeEntity.Entity;
            }
            return null;
        }

        private async Task<bool> SaveChanges()
        {
            try
            {
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }
    }
}
