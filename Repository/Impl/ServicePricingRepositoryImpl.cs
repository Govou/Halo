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

        public async Task<bool> DeleteServicePricing(ServicePricing servicePricing)
        {
            servicePricing.IsDeleted = true;
            _context.ServicePricings.Update(servicePricing);
            return await SaveChanges();
        }

        public async Task<IEnumerable<ServicePricing>> FindAllServicePricings()
        {
            return await _context.ServicePricings.AsNoTracking()
               .Include(x => x.Service)
               .Include(x => x.Branch)
               .Where(servicePricing => servicePricing.IsDeleted == false)
               .OrderBy(servicePricing => servicePricing.CreatedAt)
               .ToListAsync();
        }

        public async Task<ServicePricing> FindServicePricingById(long Id)
        {
            return await _context.ServicePricings
                .Where(servicePricing => servicePricing.IsDeleted == false)
                .FirstOrDefaultAsync(servicePricing => servicePricing.Id == Id && servicePricing.IsDeleted == false);
        }

        public async Task<IEnumerable<ServicePricing>> FindServicePricingByServiceId(long serviceId)
        {
            return await _context.ServicePricings.AsNoTracking()
                 .Include(x => x.Branch)
                 .Where(x => x.ServiceId == serviceId && x.IsDeleted == false)
                 .ToListAsync();
        }

        public async Task<IEnumerable<ServicePricing>> FindServicePricingByBranchId(long branchId)
        {
            return await _context.ServicePricings.AsNoTracking()
                 .Include(x => x.Branch)
                 .Where(x => x.BranchId == branchId && !x.IsDeleted)
                 .ToListAsync();
        }

        public async Task<ServicePricing> SaveServicePricing(ServicePricing servicePricing)
        {
            var servicePricingEntity = await _context.ServicePricings.AddAsync(servicePricing);
            if (await SaveChanges())
            {
                return servicePricingEntity.Entity;
            }
            return null;
        }

        public async Task<ServicePricing> UpdateServicePricing(ServicePricing servicePricing)
        {
            var servicePricingEntity = _context.ServicePricings.Update(servicePricing);
            if (await SaveChanges())
            {
                return servicePricingEntity.Entity;
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
