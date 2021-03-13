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
    public class StandardSLAForOperatingEntitiesRepositoryImpl : IStandardSLAForOperatingEntitiesRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<StandardSLAForOperatingEntitiesRepositoryImpl> _logger;
        public StandardSLAForOperatingEntitiesRepositoryImpl(DataContext context, ILogger<StandardSLAForOperatingEntitiesRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<StandardSLAForOperatingEntities> SaveStandardSLAForOperatingEntities(StandardSLAForOperatingEntities standardSLAForOperatingEntities)
        {
            var standardSLAForOperatingEntitiesEntity = await _context.StandardSLAForOperatingEntities.AddAsync(standardSLAForOperatingEntities);
            if(await SaveChanges())
            {
                return standardSLAForOperatingEntitiesEntity.Entity;
            }
            return null;
        }

        public async Task<StandardSLAForOperatingEntities> FindStandardSLAForOperatingEntitiesById(long Id)
        {
            return await _context.StandardSLAForOperatingEntities
                .Include(standardSLAForOperatingEntities => standardSLAForOperatingEntities.OperatingEntity)
                .FirstOrDefaultAsync( standardSLAForOperatingEntities => standardSLAForOperatingEntities.Id == Id && standardSLAForOperatingEntities.IsDeleted == false);
        }

        public async Task<StandardSLAForOperatingEntities> FindStandardSLAForOperatingEntitiesByName(string name)
        {
            return await _context.StandardSLAForOperatingEntities
                .Include(standardSLAForOperatingEntities => standardSLAForOperatingEntities.OperatingEntity)
                .FirstOrDefaultAsync( standardSLAForOperatingEntities => standardSLAForOperatingEntities.Caption == name && standardSLAForOperatingEntities.IsDeleted == false);
        }

        public async Task<IEnumerable<StandardSLAForOperatingEntities>> FindAllStandardSLAForOperatingEntities()
        {
            return await _context.StandardSLAForOperatingEntities
                .Where(standardSLAForOperatingEntities => standardSLAForOperatingEntities.IsDeleted == false)
                .Include(standardSLAForOperatingEntities => standardSLAForOperatingEntities.OperatingEntity)
                .OrderBy(standardSLAForOperatingEntities => standardSLAForOperatingEntities.CreatedAt)
                .ToListAsync();
        }

        public async Task<StandardSLAForOperatingEntities> UpdateStandardSLAForOperatingEntities(StandardSLAForOperatingEntities standardSLAForOperatingEntities)
        {
            var standardSLAForOperatingEntitiesEntity =  _context.StandardSLAForOperatingEntities.Update(standardSLAForOperatingEntities);
            if(await SaveChanges())
            {
                return standardSLAForOperatingEntitiesEntity.Entity;
            }
            return null;
        }

        public async Task<bool> DeleteStandardSLAForOperatingEntities(StandardSLAForOperatingEntities standardSLAForOperatingEntities)
        {
            standardSLAForOperatingEntities.IsDeleted = true;
            _context.StandardSLAForOperatingEntities.Update(standardSLAForOperatingEntities);
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