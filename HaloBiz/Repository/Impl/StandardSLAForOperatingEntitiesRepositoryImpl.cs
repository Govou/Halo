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
    public class StandardSlaforOperatingEntityRepositoryImpl : IStandardSlaforOperatingEntityRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<StandardSlaforOperatingEntityRepositoryImpl> _logger;
        public StandardSlaforOperatingEntityRepositoryImpl(HalobizContext context, ILogger<StandardSlaforOperatingEntityRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<StandardSlaforOperatingEntity> SaveStandardSlaforOperatingEntity(StandardSlaforOperatingEntity standardSLAForOperatingEntities)
        {
            var standardSLAForOperatingEntitiesEntity = await _context.StandardSlaforOperatingEntities.AddAsync(standardSLAForOperatingEntities);
            if(await SaveChanges())
            {
                return standardSLAForOperatingEntitiesEntity.Entity;
            }
            return null;
        }

        public async Task<StandardSlaforOperatingEntity> FindStandardSlaforOperatingEntityById(long Id)
        {
            return await _context.StandardSlaforOperatingEntities
                .Include(standardSLAForOperatingEntities => standardSLAForOperatingEntities.OperatingEntity)
                .FirstOrDefaultAsync( standardSLAForOperatingEntities => standardSLAForOperatingEntities.Id == Id && standardSLAForOperatingEntities.IsDeleted == false);
        }

        public async Task<StandardSlaforOperatingEntity> FindStandardSlaforOperatingEntityByName(string name)
        {
            return await _context.StandardSlaforOperatingEntities
                .Include(standardSLAForOperatingEntities => standardSLAForOperatingEntities.OperatingEntity)
                .FirstOrDefaultAsync( standardSLAForOperatingEntities => standardSLAForOperatingEntities.Caption == name && standardSLAForOperatingEntities.IsDeleted == false);
        }

        public async Task<IEnumerable<StandardSlaforOperatingEntity>> FindAllStandardSlaforOperatingEntity()
        {
            return await _context.StandardSlaforOperatingEntities
                .Where(standardSLAForOperatingEntities => standardSLAForOperatingEntities.IsDeleted == false)
                .Include(standardSLAForOperatingEntities => standardSLAForOperatingEntities.OperatingEntity)
                .OrderBy(standardSLAForOperatingEntities => standardSLAForOperatingEntities.CreatedAt)
                .ToListAsync();
        }

        public async Task<StandardSlaforOperatingEntity> UpdateStandardSlaforOperatingEntity(StandardSlaforOperatingEntity standardSLAForOperatingEntities)
        {
            var standardSLAForOperatingEntitiesEntity =  _context.StandardSlaforOperatingEntities.Update(standardSLAForOperatingEntities);
            if(await SaveChanges())
            {
                return standardSLAForOperatingEntitiesEntity.Entity;
            }
            return null;
        }

        public async Task<bool> DeleteStandardSlaforOperatingEntity(StandardSlaforOperatingEntity standardSLAForOperatingEntities)
        {
            standardSLAForOperatingEntities.IsDeleted = true;
            _context.StandardSlaforOperatingEntities.Update(standardSLAForOperatingEntities);
            return await SaveChanges();
        }
        private async Task<bool> SaveChanges()
        {
           try
           {
               return  await _context.SaveChangesAsync() > 0;
           }
           catch(Exception ex)
           {
               _logger.LogError(ex.Message);
               return false;
           }
        }
    }
}