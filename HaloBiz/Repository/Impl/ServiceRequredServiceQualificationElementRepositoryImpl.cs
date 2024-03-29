using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HalobizMigrations.Data;
using HalobizMigrations.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HaloBiz.Repository.Impl
{
    public class ServiceRequredServiceQualificationElementRepositoryImpl : IServiceRequredServiceQualificationElementRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<ServiceRequredServiceQualificationElementRepositoryImpl> _logger;
        public ServiceRequredServiceQualificationElementRepositoryImpl(HalobizContext context, ILogger<ServiceRequredServiceQualificationElementRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<bool> SaveRangeServiceRequredServiceQualificationElement(IEnumerable<ServiceRequredServiceQualificationElement> serviceRequredServiceQualificationElement)
        {
            try
            {
                await _context.ServiceRequredServiceQualificationElements.AddRangeAsync(serviceRequredServiceQualificationElement);
                var affected = await _context.SaveChangesAsync();
                return affected > 0 ? true : false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
            }

            return false;
        }
        public async Task<ServiceRequredServiceQualificationElement> SaveServiceRequredServiceQualificationElement(ServiceRequredServiceQualificationElement serviceRequredServiceQualificationElement)
        {
            var savedEntity = await _context.ServiceRequredServiceQualificationElements.AddAsync(serviceRequredServiceQualificationElement);
            if(await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<ServiceRequredServiceQualificationElement> FindServiceRequredServiceQualificationElementById(long serviceId, long serviceElementId)
        {
            return await _context.ServiceRequredServiceQualificationElements                
                .FirstOrDefaultAsync( serviceRequredServiceQualificationElement => 
                    serviceRequredServiceQualificationElement.ServicesId == serviceId 
                        && serviceRequredServiceQualificationElement.RequredServiceQualificationElementId== serviceElementId 
                        && serviceRequredServiceQualificationElement.IsDeleted == false);
        }

        public async Task<bool> DeleteServiceRequredServiceQualificationElement(ServiceRequredServiceQualificationElement serviceRequredServiceQualificationElement)
        {
            serviceRequredServiceQualificationElement.IsDeleted = true;
            _context.ServiceRequredServiceQualificationElements.Update(serviceRequredServiceQualificationElement);
            return await SaveChanges();
        }

         public async Task<bool> DeleteRangeServiceRequredServiceQualificationElement(IEnumerable<ServiceRequredServiceQualificationElement> serviceRequredServiceQualificationElements)
        {
            foreach (ServiceRequredServiceQualificationElement doc in serviceRequredServiceQualificationElements)
            {
                doc.IsDeleted = true;
            }
            _context.ServiceRequredServiceQualificationElements.UpdateRange(serviceRequredServiceQualificationElements);
            return await SaveChanges();
        }

        private async Task<bool> SaveChanges()
        {
        try{
               return  await _context.SaveChangesAsync() > 0;
           }catch(Exception ex)
           {
               _logger.LogError(ex.Message);
               return false;
           }
        }
    }
}