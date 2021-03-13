using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HaloBiz.Data;
using HaloBiz.Model.ManyToManyRelationship;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HaloBiz.Repository.Impl
{
    public class ServiceRequredServiceQualificationElementRepositoryImpl : IServiceRequredServiceQualificationElementRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<ServiceRequredServiceQualificationElementRepositoryImpl> _logger;
        public ServiceRequredServiceQualificationElementRepositoryImpl(DataContext context, ILogger<ServiceRequredServiceQualificationElementRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<bool> SaveRangeServiceRequredServiceQualificationElement(IEnumerable<ServiceRequredServiceQualificationElement> serviceRequredServiceQualificationElement)
        {
            await _context.ServiceRequredServiceQualificationElement.AddRangeAsync(serviceRequredServiceQualificationElement);
            return await SaveChanges();
        }
        public async Task<ServiceRequredServiceQualificationElement> SaveServiceRequredServiceQualificationElement(ServiceRequredServiceQualificationElement serviceRequredServiceQualificationElement)
        {
            var savedEntity = await _context.ServiceRequredServiceQualificationElement.AddAsync(serviceRequredServiceQualificationElement);
            if(await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<ServiceRequredServiceQualificationElement> FindServiceRequredServiceQualificationElementById(long serviceId, long serviceElementId)
        {
            return await _context.ServiceRequredServiceQualificationElement                
                .FirstOrDefaultAsync( serviceRequredServiceQualificationElement => 
                    serviceRequredServiceQualificationElement.ServicesId == serviceId 
                        && serviceRequredServiceQualificationElement.RequredServiceQualificationElementId== serviceElementId 
                        && serviceRequredServiceQualificationElement.IsDeleted == false);
        }

        public async Task<bool> DeleteServiceRequredServiceQualificationElement(ServiceRequredServiceQualificationElement serviceRequredServiceQualificationElement)
        {
            serviceRequredServiceQualificationElement.IsDeleted = true;
            _context.ServiceRequredServiceQualificationElement.Update(serviceRequredServiceQualificationElement);
            return await SaveChanges();
        }

         public async Task<bool> DeleteRangeServiceRequredServiceQualificationElement(IEnumerable<ServiceRequredServiceQualificationElement> serviceRequredServiceQualificationElements)
        {
            foreach (ServiceRequredServiceQualificationElement doc in serviceRequredServiceQualificationElements)
            {
                doc.IsDeleted = true;
            }
            _context.ServiceRequredServiceQualificationElement.UpdateRange(serviceRequredServiceQualificationElements);
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