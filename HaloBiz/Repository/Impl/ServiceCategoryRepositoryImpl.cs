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
    public class ServiceCategoryRepositoryImpl : IServiceCategoryRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<ServiceCategoryRepositoryImpl> _logger;
        public ServiceCategoryRepositoryImpl( HalobizContext context, ILogger<ServiceCategoryRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<ServiceCategory> SaveServiceCategory(ServiceCategory serviceCategory)
        {
            //check if this previously exist
            if(_context.ServiceCategories.Any(x=>x.Name.Trim().ToLower() == serviceCategory.Name.Trim().ToLower() && x.ServiceGroupId == serviceCategory.ServiceGroupId && x.IsDeleted==false))
            {
                //record previously exist
                return null;
            }

            var savedEntity = await _context.ServiceCategories.AddAsync(serviceCategory);
            if(await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<ServiceCategory> FindServiceCategoryById(long Id)
        {
            var serviceCategory = await _context.ServiceCategories
                .Where(category => category.Id == Id && category.IsDeleted == false)
                .Include(x => x.Services
                    .Where(x => x.IsDeleted == false && x.IsPublished == true))
                .ThenInclude(x=>x.AdminRelationship)
                .Include(x => x.Services
                    .Where(x => x.IsDeleted == false && x.IsPublished == true))
                .ThenInclude(x=>x.DirectRelationship)
                .FirstOrDefaultAsync();
            
            if(serviceCategory == null)
            {
                return null;
            }

            serviceCategory.ServiceCategoryTasks = await _context.ServiceCategoryTasks
                    .Where(x => x.ServiceCategoryId == serviceCategory.Id && !x.IsDeleted).ToListAsync();
            
            if(serviceCategory.ServiceGroupId > 0)
            {
                serviceCategory.ServiceGroup = await _context.ServiceGroups
                        .FirstOrDefaultAsync(x => x.Id == serviceCategory.ServiceGroupId);
            }          

            return serviceCategory;
        }

        public async Task<ServiceCategory> FindServiceCategoryByName(string name)
        {
            var serviceCategory = await _context.ServiceCategories
                .Include(serviceCategory => serviceCategory.ServiceGroup)
                .Include(serviceCategory => serviceCategory.ServiceCategoryTasks
                    .Where(serviceCategoryTask => serviceCategoryTask.IsDeleted == false))                
                .FirstOrDefaultAsync( category => category.Name == name && category.IsDeleted == false);

                if(serviceCategory != null){
                    serviceCategory.Services = await _context.Services
                    .Include(x => x.ServiceRequiredServiceDocuments).ThenInclude(x => x.RequiredServiceDocument)
                    .Include(x => x.ServiceRequredServiceQualificationElements).ThenInclude(x => x.RequredServiceQualificationElement)
                    .Where( service => service.ServiceCategoryId == serviceCategory.Id && service.IsDeleted == false)
                    .ToListAsync();
                }

                return serviceCategory;        
        }

        public async Task<IEnumerable<ServiceCategory>> FindAllServiceCategories()
        {
            return await _context.ServiceCategories.Where(category => category.IsDeleted == false)
                .Include(serviceCategory => serviceCategory.ServiceGroup)
                .Include(category => category.Services
                    .Where(service => service.IsDeleted == false))
                .Include(serviceCategory => serviceCategory.ServiceCategoryTasks
                    .Where(serviceCategoryTask => serviceCategoryTask.IsDeleted == false))
                    .ThenInclude(serviceCategoryTask => serviceCategoryTask.ServiceTaskDeliverables)
                .ToListAsync();
        }

        public async Task<ServiceCategory> UpdateServiceCategory(ServiceCategory category)
        {
            var updatedEntity =  _context.ServiceCategories.Update(category);
            if(await SaveChanges())
            {
                return updatedEntity.Entity;
            }
            return null;
        }

        public async Task<bool> DeleteServiceCategory(ServiceCategory category)
        {
            category.IsDeleted = true;
            _context.ServiceCategories.Update(category);
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

        public async Task<bool> DeleteServiceCategoryRange(IEnumerable<ServiceCategory> serviceCategories)
        {
            foreach (var sc in serviceCategories)
            {
                sc.IsDeleted = true;
            }
            _context.ServiceCategories.UpdateRange(serviceCategories);
            return await SaveChanges();
        }
    }
}