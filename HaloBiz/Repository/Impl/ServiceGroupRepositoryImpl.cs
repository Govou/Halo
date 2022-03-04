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
    public class ServiceGroupRepositoryImpl : IServiceGroupRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<ServiceGroupRepositoryImpl> _logger;
        private readonly IServiceCategoryRepository _serviceCategoryRepository;
        public ServiceGroupRepositoryImpl(HalobizContext context, ILogger<ServiceGroupRepositoryImpl> logger, IServiceCategoryRepository serviceCategoryRepository)
        {
            this._logger = logger;
            this._context = context;
            _serviceCategoryRepository = serviceCategoryRepository;
        }

        public async Task<ServiceGroup> SaveServiceGroup(ServiceGroup serviceGroup)
        {
            var savedEntity = await _context.ServiceGroups.AddAsync(serviceGroup);
            if(await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<ServiceGroup> FindServiceGroupById(long Id)
        {
            return await _context.ServiceGroups
                .Include(serviceGroup => serviceGroup.OperatingEntity)
                .Include(serviceGroup => serviceGroup.ServiceCategories)                 
                .FirstOrDefaultAsync( serviceGroup => serviceGroup.Id == Id && serviceGroup.IsDeleted == false);
        }

        public async Task<ServiceGroup> FindServiceGroupByName(string name)
        {
            return await _context.ServiceGroups
                .Include(serviceGroup => serviceGroup.OperatingEntity)
                .Include(serviceGroup => serviceGroup.ServiceCategories) 
                .FirstOrDefaultAsync( serviceGroup => serviceGroup.Name == name && serviceGroup.IsDeleted == false);
        }

        public async Task<IEnumerable<ServiceGroup>> FindAllServiceGroups()
        {
            return await _context.ServiceGroups.Where(serviceGroup => serviceGroup.IsDeleted == false)
                .Include(serviceGroup => serviceGroup.OperatingEntity)
                .Include(serviceGroup => serviceGroup.ServiceCategories)
                .ToListAsync();
        }

        public async Task<ServiceGroup> UpdateServiceGroup(ServiceGroup serviceGroup)
        {
            var updatedEntity =  _context.ServiceGroups.Update(serviceGroup);
            if(await SaveChanges())
            {
                return updatedEntity.Entity;
            }
            return null;
        }

        public async Task<bool> DeleteServiceGroup(ServiceGroup serviceGroup)
        {
            await _serviceCategoryRepository.DeleteServiceCategoryRange(serviceGroup.ServiceCategories);

            serviceGroup.IsDeleted = true;
            _context.ServiceGroups.Update(serviceGroup);
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

        public async Task<bool> DeleteServiceGroupRange(IEnumerable<ServiceGroup> serviceGroups)
        {
            foreach (var sg in serviceGroups)
            {
                await _serviceCategoryRepository.DeleteServiceCategoryRange(sg.ServiceCategories);
                sg.IsDeleted = true;
            }
            _context.ServiceGroups.UpdateRange(serviceGroups);
            return await SaveChanges();

        }
    }
}