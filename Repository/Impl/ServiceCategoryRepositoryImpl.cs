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
    public class ServiceCategoryRepositoryImpl : IServiceCategoryRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<ServiceCategoryRepositoryImpl> _logger;
        public ServiceCategoryRepositoryImpl(DataContext context, ILogger<ServiceCategoryRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<ServiceCategory> SaveServiceCategory(ServiceCategory serviceCategory)
        {
            var savedEntity = await _context.ServiceCategories.AddAsync(serviceCategory);
            if(await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<ServiceCategory> FindServiceCategoryById(long Id)
        {
            return await _context.ServiceCategories
                .Include(office => office.ServiceGroup)
                .Include(office => office.Services)                 
                .FirstOrDefaultAsync( category => category.Id == Id && category.IsDeleted == false);
        }

        public async Task<ServiceCategory> FindServiceCategoryByName(string name)
        {
            return await _context.ServiceCategories
                .Include(office => office.ServiceGroup)
                .Include(category => category.Services)
                .FirstOrDefaultAsync(category => category.Name == name && category.IsDeleted == false);
        }

        public async Task<IEnumerable<ServiceCategory>> FindAllServiceCategories()
        {
            return await _context.ServiceCategories.Where(category => category.IsDeleted == false)
                .Include(office => office.ServiceGroup)
                .Include(category => category.Services)
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
    }
}