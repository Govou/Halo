using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaloBiz.Data;
using HaloBiz.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HaloBiz.Repository.Impl
{
    public class ServicesRepositoryImpl : IServicesRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<ServicesRepositoryImpl> _logger;

        public ServicesRepositoryImpl(DataContext context, ILogger<ServicesRepositoryImpl> logger)
        {
            this._context = context;
            this._logger = logger;
        }

         public async Task<Services> SaveService(Services service)
        {
            var savedEntity = await _context.Services.AddAsync(service);
            if(! await SaveChanges())
            {
                return null;
            }
            var savedService = savedEntity.Entity;
            var serviceCode = $"{savedService.DivisionId}/{savedService.OperatingEntityId}/{savedService.ServiceGroupId}/{savedService.ServiceCategoryId}/{savedService.Id}";
            savedService.ServiceCode = serviceCode;
            
            return await UpdateServices(savedService);
        }

        public async Task<Services> FindServicesById(long Id)
        {
            return await _context.Services
                .Include(service => service.Target)
                .Include(service => service.ServiceType)
                .Include(service => service.RequiredServiceDocument.Where(row => row.IsDeleted == false))
                    .ThenInclude(row => row.RequiredServiceDocument)
                .Include(service => service.RequredServiceQualificationElement.Where(row => row.IsDeleted == false))
                .ThenInclude(row => row.RequredServiceQualificationElement)
                .FirstOrDefaultAsync( service => service.Id == Id && service.IsDeleted == false);
        }

        public async Task<Services> FindServiceByName(string name)
        {
            return await _context.Services
                .Include(service => service.Target)
                .Include(service => service.ServiceType)
                .Include(service => service.RequiredServiceDocument.Where(row => row.IsDeleted == false))
                    .ThenInclude(row => row.RequiredServiceDocument)
                .Include(service => service.RequredServiceQualificationElement.Where(row => row.IsDeleted == false))
                    .ThenInclude(row => row.RequredServiceQualificationElement)
                .FirstOrDefaultAsync( service => service.Name == name && service.IsDeleted == false);
        }

        public async Task<IEnumerable<Services>> FindAllServices()
        {
            return await _context.Services
                .Include(service => service.Target)
                .Include(service => service.ServiceType)
                .Include(service => service.RequiredServiceDocument.Where(row => row.IsDeleted == false))
                    .ThenInclude(row => row.RequiredServiceDocument)
                    .Include(service => service.RequredServiceQualificationElement.Where(row => row.IsDeleted == false))
                    .ThenInclude(row => row.RequredServiceQualificationElement)
                .Where(service => service.IsDeleted == false)
                    .ToListAsync();
        }

        public async Task<Services> UpdateServices(Services service)
        {
            var updatedEntity =  _context.Services.Update(service);
            if(await SaveChanges())
            {
                return updatedEntity.Entity;
            }
            return null;
        }

        public async Task<bool> DeleteService(Services service)
        {
            service.IsDeleted = true;
            _context.Services.Update(service);
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