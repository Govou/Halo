using HalobizMigrations.Data;
using HalobizMigrations.Models;
using HalobizMigrations.Models.Halobiz;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository.Impl
{
    public class ServiceQualificationRepositoryImpl : IServiceQualificationRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<ServiceQualificationRepositoryImpl> _logger;
        public ServiceQualificationRepositoryImpl(HalobizContext context, ILogger<ServiceQualificationRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<bool> DeleteServiceQualification(ServiceQualification serviceQualification)
        {
            serviceQualification.IsDeleted = true;
            _context.ServiceQualifications.Update(serviceQualification);
            return await SaveChanges();
        }

        public async Task<IEnumerable<ServiceQualification>> FindAllServiceQualifications()
        {
            return await _context.ServiceQualifications
               .Where(serviceQualification => serviceQualification.IsDeleted == false)
               .OrderBy(serviceQualification => serviceQualification.CreatedAt)
               .ToListAsync();
        }

        public async Task<ServiceQualification> FindServiceQualificationById(long Id)
        {
            return await _context.ServiceQualifications
                .Where(serviceQualification => serviceQualification.IsDeleted == false)
                .FirstOrDefaultAsync(serviceQualification => serviceQualification.Id == Id && serviceQualification.IsDeleted == false);

        }

        /*public async Task<ServiceQualification> FindServiceQualificationByName(string name)
        {
            return await _context.ServiceQualifications
                 .Where(serviceQualification => serviceQualification.IsDeleted == false)
                 .FirstOrDefaultAsync(serviceQualification => serviceQualification.Caption == name && serviceQualification.IsDeleted == false);

        }*/

        public async Task<ServiceQualification> SaveServiceQualification(ServiceQualification serviceQualification)
        {
            var serviceQualificationEntity = await _context.ServiceQualifications.AddAsync(serviceQualification);
            if (await SaveChanges())
            {
                return serviceQualificationEntity.Entity;
            }
            return null;
        }

        public async Task<ServiceQualification> UpdateServiceQualification(ServiceQualification serviceQualification)
        {
            var serviceQualificationEntity = _context.ServiceQualifications.Update(serviceQualification);
            if (await SaveChanges())
            {
                return serviceQualificationEntity.Entity;
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
