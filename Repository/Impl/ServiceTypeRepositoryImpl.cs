using HaloBiz.Data;
using HaloBiz.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository.Impl
{
    public class ServiceTypeRepositoryImpl : IServiceTypeRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<ServiceTypeRepositoryImpl> _logger;
        public ServiceTypeRepositoryImpl(DataContext context, ILogger<ServiceTypeRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<bool> DeleteServiceType(ServiceType serviceType)
        {
            serviceType.IsDeleted = true;
            _context.ServiceTypes.Update(serviceType);
            return await SaveChanges();
        }

        public async Task<IEnumerable<ServiceType>> FindAllServiceTypes()
        {
            return await _context.ServiceTypes
               .Where(serviceType => serviceType.IsDeleted == false)
               .OrderBy(serviceType => serviceType.CreatedAt)
               .ToListAsync();
        }

        public async Task<ServiceType> FindServiceTypeById(long Id)
        {
            return await _context.ServiceTypes
                .Where(serviceType => serviceType.IsDeleted == false)
                .FirstOrDefaultAsync(serviceType => serviceType.Id == Id && serviceType.IsDeleted == false);

        }

        public async Task<ServiceType> FindServiceTypeByName(string name)
        {
            return await _context.ServiceTypes
                 .Where(serviceType => serviceType.IsDeleted == false)
                 .FirstOrDefaultAsync(serviceType => serviceType.Caption == name && serviceType.IsDeleted == false);

        }

        public async Task<ServiceType> SaveServiceType(ServiceType serviceType)
        {
            var serviceTypeEntity = await _context.ServiceTypes.AddAsync(serviceType);
            if (await SaveChanges())
            {
                return serviceTypeEntity.Entity;
            }
            return null;
        }

        public async Task<ServiceType> UpdateServiceType(ServiceType serviceType)
        {
            var serviceTypeEntity = _context.ServiceTypes.Update(serviceType);
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
