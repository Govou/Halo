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
    public class OperatingEntityRepositoryImpl : IOperatingEntityRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<OperatingEntityRepositoryImpl> _logger;
        private readonly IServiceGroupRepository _serviceGroupRepository;
        public OperatingEntityRepositoryImpl(DataContext context, ILogger<OperatingEntityRepositoryImpl> logger, IServiceGroupRepository serviceGroupRepository)
        {
            this._logger = logger;
            this._context = context;
            _serviceGroupRepository = serviceGroupRepository;
        }

        public async Task<OperatingEntity> SaveOperatingEntity(OperatingEntity operatingEntity)
        {
            var savedEntity = await _context.OperatingEntities
                .AddAsync(operatingEntity);
                
            if(await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<OperatingEntity> FindOperatingEntityById(long Id)
        {
            return await _context.OperatingEntities
                .Include(operatingEntity => operatingEntity.Head)
                .Include(operatingEntity => operatingEntity.ServiceGroups)
                .ThenInclude(x => x.ServiceCategories)
                .Include(operatingEntity => operatingEntity.Division)
                .Include(operatingEntity => operatingEntity.StrategicBusinessUnits)        
                .FirstOrDefaultAsync( operatingEntity => operatingEntity.Id == Id && operatingEntity.IsDeleted == false);
        }

        public async Task<OperatingEntity> FindOperatingEntityByName(string name)
        {
            return await _context.OperatingEntities
                .Include(operatingEntity => operatingEntity.Head)
                .Include(operatingEntity => operatingEntity.ServiceGroups)
                .ThenInclude(x => x.ServiceCategories)
                .Include(operatingEntity => operatingEntity.Division)
                .Include(operatingEntity => operatingEntity.StrategicBusinessUnits)        
                .FirstOrDefaultAsync( operatingEntity => operatingEntity.Name == name && operatingEntity.IsDeleted == false);
        }

        public async Task<IEnumerable<OperatingEntity>> FindAllOperatingEntity()
        {
            return await _context.OperatingEntities.Where(operatingEntity => operatingEntity.IsDeleted == false)
                .Include(operatingEntity => operatingEntity.Head)
                .Include(operatingEntity => operatingEntity.ServiceGroups)
                .ThenInclude(x => x.ServiceCategories)
                .Include(operatingEntity => operatingEntity.Division)
                .Include(operatingEntity => operatingEntity.StrategicBusinessUnits)   
                .ToListAsync();
        }

        public async Task<IEnumerable<OperatingEntity>> FindAllOperatingEntityWithSBUProportion()
        {
            var operatingEntites =  await _context.OperatingEntities
                            .Where(x => !x.IsDeleted).ToListAsync();

            foreach (var operatingEntity in operatingEntites)
            {
                operatingEntity.SBUProportion = await _context.SBUProportions
                    .FirstOrDefaultAsync(x => x.OperatingEntityId == operatingEntity.Id && !x.IsDeleted);
            }
            return operatingEntites;
        }


        public async Task<OperatingEntity> UpdateOperatingEntity(OperatingEntity operatingEntity)
        {
            var updatedEntity =  _context.OperatingEntities.Update(operatingEntity);
            if(await SaveChanges())
            {
                return updatedEntity.Entity;
            }
            return null;
        }

        public async Task<bool> DeleteOperatingEntity(OperatingEntity operatingEntity)
        {
            await _serviceGroupRepository.DeleteServiceGroupRange(operatingEntity.ServiceGroups);

            operatingEntity.IsDeleted = true;
            _context.OperatingEntities.Update(operatingEntity);
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

        public async Task<bool> DeleteOperatingEntityRange(IEnumerable<OperatingEntity> operatingEntities)
        {
            foreach (var oe in operatingEntities)
            {
                await _serviceGroupRepository.DeleteServiceGroupRange(oe.ServiceGroups);
                oe.IsDeleted = true;
            }
            _context.OperatingEntities.UpdateRange(operatingEntities);
            return await SaveChanges();
        }
    }
}