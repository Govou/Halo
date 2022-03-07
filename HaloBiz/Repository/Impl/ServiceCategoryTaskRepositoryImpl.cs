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
    public class ServiceCategoryTaskRepositoryImpl : IServiceCategoryTaskRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<ServiceCategoryTaskRepositoryImpl> _logger;
        private readonly IServiceTaskDeliverableRepository _serviceTaskDeliverableRepository;
        public ServiceCategoryTaskRepositoryImpl(HalobizContext context, ILogger<ServiceCategoryTaskRepositoryImpl> logger, IServiceTaskDeliverableRepository serviceTaskDeliverableRepository)
        {
            this._logger = logger;
            this._context = context;
            _serviceTaskDeliverableRepository = serviceTaskDeliverableRepository;
        }

        public async Task<ServiceCategoryTask> SaveServiceCategoryTask(ServiceCategoryTask serviceCategoryTask)
        {
            var savedEntity = await _context.ServiceCategoryTasks.AddAsync(serviceCategoryTask);
            if(await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<ServiceCategoryTask> FindServiceCategoryTaskById(long Id)
        {
            return await _context.ServiceCategoryTasks
                .Include(serviceCategoryTask => serviceCategoryTask.ServiceCategory)
                .Include(serviceCategoryTask => serviceCategoryTask.ServiceTaskDeliverables)
                .FirstOrDefaultAsync( serviceCategoryTask => serviceCategoryTask.Id == Id && serviceCategoryTask.IsDeleted == false);
        }

        public async Task<ServiceCategoryTask> FindServiceCategoryTaskByName(string caption)
        {
            return await _context.ServiceCategoryTasks
                .Include(serviceCategoryTask => serviceCategoryTask.ServiceCategory) 
                .Include(serviceCategoryTask => serviceCategoryTask.ServiceTaskDeliverables)
                .FirstOrDefaultAsync( serviceCategoryTask => serviceCategoryTask.Caption == caption && serviceCategoryTask.IsDeleted == false);
        }

        public async Task<IEnumerable<ServiceCategoryTask>> FindAllServiceCategoryTasks()
        {
            return await _context.ServiceCategoryTasks.Where(serviceCategoryTask => serviceCategoryTask.IsDeleted == false)
                .Include(serviceCategoryTask => serviceCategoryTask.ServiceCategory)
                .Include(serviceCategoryTask => serviceCategoryTask.EndorsementType)
                .Include(serviceCategoryTask => serviceCategoryTask.ServiceTaskDeliverables)
                .ToListAsync();
        }

        public async Task<ServiceCategoryTask> UpdateServiceCategoryTask(ServiceCategoryTask serviceCategoryTask)
        {
            var updatedEntity =  _context.ServiceCategoryTasks.Update(serviceCategoryTask);
            if(await SaveChanges())
            {
                return updatedEntity.Entity;
            }
            return null;
        }

        public async Task<bool> DeleteServiceCategoryTask(ServiceCategoryTask serviceCategoryTask)
        {
            await _serviceTaskDeliverableRepository.DeleteServiceTaskDeliverableRange(serviceCategoryTask.ServiceTaskDeliverables);
            serviceCategoryTask.IsDeleted = true;
            _context.ServiceCategoryTasks.Update(serviceCategoryTask);
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

        public async Task<bool> DeleteServiceCategoryTaskRange(IEnumerable<ServiceCategoryTask> serviceCategoryTasks)
        {
            foreach (var serviceTaskDeliverable in serviceCategoryTasks)
            {
                await _serviceTaskDeliverableRepository.DeleteServiceTaskDeliverableRange(serviceTaskDeliverable.ServiceTaskDeliverables);
                serviceTaskDeliverable.IsDeleted = true;
            }
            _context.ServiceCategoryTasks.UpdateRange(serviceCategoryTasks);
            return await SaveChanges();

        }
        
    }
}