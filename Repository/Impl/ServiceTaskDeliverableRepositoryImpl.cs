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
    public class ServiceTaskDeliverableRepositoryImpl : IServiceTaskDeliverableRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<ServiceTaskDeliverableRepositoryImpl> _logger;
        private readonly IServiceCategoryRepository _serviceCategoryRepository;
        public ServiceTaskDeliverableRepositoryImpl(DataContext context, ILogger<ServiceTaskDeliverableRepositoryImpl> logger, IServiceCategoryRepository serviceCategoryRepository)
        {
            this._logger = logger;
            this._context = context;
            _serviceCategoryRepository = serviceCategoryRepository;
        }

        public async Task<ServiceTaskDeliverable> SaveServiceTaskDeliverable(ServiceTaskDeliverable serviceTaskDeliverable)
        {
            var savedEntity = await _context.ServiceTaskDeliverables.AddAsync(serviceTaskDeliverable);
            if(await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<ServiceTaskDeliverable> FindServiceTaskDeliverableById(long Id)
        {
            return await _context.ServiceTaskDeliverables
                .Include(serviceTaskDeliverable => serviceTaskDeliverable.ServiceCategoryTask)
                .FirstOrDefaultAsync( serviceTaskDeliverable => serviceTaskDeliverable.Id == Id && serviceTaskDeliverable.IsDeleted == false);
        }

        public async Task<ServiceTaskDeliverable> FindServiceTaskDeliverableByName(string caption)
        {
            return await _context.ServiceTaskDeliverables
                .Include(serviceTaskDeliverable => serviceTaskDeliverable.ServiceCategoryTask) 
                .FirstOrDefaultAsync( serviceTaskDeliverable => serviceTaskDeliverable.Caption == caption && serviceTaskDeliverable.IsDeleted == false);
        }

        public async Task<IEnumerable<ServiceTaskDeliverable>> FindAllServiceTaskDeliverables()
        {
            return await _context.ServiceTaskDeliverables.Where(serviceTaskDeliverable => serviceTaskDeliverable.IsDeleted == false)
                .Include(serviceTaskDeliverable => serviceTaskDeliverable.ServiceCategoryTask)
                .ToListAsync();
        }

        public async Task<ServiceTaskDeliverable> UpdateServiceTaskDeliverable(ServiceTaskDeliverable serviceTaskDeliverable)
        {
            var updatedEntity =  _context.ServiceTaskDeliverables.Update(serviceTaskDeliverable);
            if(await SaveChanges())
            {
                return updatedEntity.Entity;
            }
            return null;
        }

        public async Task<bool> DeleteServiceTaskDeliverable(ServiceTaskDeliverable serviceTaskDeliverable)
        {
            serviceTaskDeliverable.IsDeleted = true;
            _context.ServiceTaskDeliverables.Update(serviceTaskDeliverable);
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

        public async Task<bool> DeleteServiceTaskDeliverableRange(IEnumerable<ServiceTaskDeliverable> serviceTaskDeliverable)
        {
            foreach (var sg in serviceTaskDeliverable)
            {
                sg.IsDeleted = true;
            }
            _context.ServiceTaskDeliverables.UpdateRange(serviceTaskDeliverable);
            return await SaveChanges();

        }

        
    }
}