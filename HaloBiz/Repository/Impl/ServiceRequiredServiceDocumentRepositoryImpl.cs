using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HalobizMigrations.Data;
using HalobizMigrations.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HaloBiz.Repository.Impl
{
    public class ServiceRequiredServiceDocumentRepositoryImpl : IServiceRequiredServiceDocumentRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<ServiceRequiredServiceDocumentRepositoryImpl> _logger;
        public ServiceRequiredServiceDocumentRepositoryImpl(HalobizContext context, ILogger<ServiceRequiredServiceDocumentRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<bool> SaveRangeServiceRequiredServiceDocument(IEnumerable<ServiceRequiredServiceDocument> serviceRequiredServiceDocuments)
        {
            await _context.ServiceRequiredServiceDocuments.AddRangeAsync(serviceRequiredServiceDocuments);
            return await SaveChanges();
        }
        public async Task<ServiceRequiredServiceDocument> SaveServiceRequiredServiceDocument(ServiceRequiredServiceDocument serviceRequiredServiceDocument)
        {
            var savedEntity = await _context.ServiceRequiredServiceDocuments.AddAsync(serviceRequiredServiceDocument);
            if(await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<ServiceRequiredServiceDocument> FindServiceRequiredServiceDocumentById(long serviceId, long serviceDocumentId)
        {
            return await _context.ServiceRequiredServiceDocuments               
                .FirstOrDefaultAsync( serviceRequiredServiceDocument => 
                    serviceRequiredServiceDocument.ServicesId == serviceId 
                        && serviceRequiredServiceDocument.RequiredServiceDocumentId== serviceDocumentId 
                        && serviceRequiredServiceDocument.IsDeleted == false);
        }

        public async Task<bool> DeleteServiceRequiredServiceDocument(ServiceRequiredServiceDocument serviceRequiredServiceDocument)
        {
            serviceRequiredServiceDocument.IsDeleted = true;
            _context.ServiceRequiredServiceDocuments.Update(serviceRequiredServiceDocument);
            return await SaveChanges();
        }

         public async Task<bool> DeleteRangeServiceRequiredServiceDocument(IEnumerable<ServiceRequiredServiceDocument> serviceRequiredServiceDocuments)
        {
            foreach (ServiceRequiredServiceDocument doc in serviceRequiredServiceDocuments)
            {
                doc.IsDeleted = true;
            }
            _context.ServiceRequiredServiceDocuments.UpdateRange(serviceRequiredServiceDocuments);
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