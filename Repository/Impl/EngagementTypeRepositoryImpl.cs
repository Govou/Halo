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

    public class EngagementTypeRepositoryImpl : IEngagementTypeRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<EngagementTypeRepositoryImpl> _logger;
        public EngagementTypeRepositoryImpl(HalobizContext context, ILogger<EngagementTypeRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }
        public async Task<EngagementType> SaveEngagementType(EngagementType engagementType)
        {
            var engagementTypeEntity = await _context.EngagementTypes.AddAsync(engagementType);
            if(await SaveChanges())
            {
                return engagementTypeEntity.Entity;
            }
            return null;
        }
        public async Task<IEnumerable<EngagementType>> GetEngagementTypes()
        {
            return await _context.EngagementTypes
                .Where(x => x.IsDeleted == false)
                .OrderBy(x => x.Caption)
                .ToListAsync();
        }

        public async Task<EngagementType> UpdateEngagementType(EngagementType engagementType)
        {
             var engagementTypeEntity =  _context.EngagementTypes.Update(engagementType);
            if(await SaveChanges())
            {
                return engagementTypeEntity.Entity;
            }
            return null;
        }
        public async Task<bool> DeleteEngagementType(EngagementType engagementType)
        {
            engagementType.IsDeleted = true;
            _context.EngagementTypes.Update(engagementType);
            return await SaveChanges();
        }
        public async Task<EngagementType> FindEngagementTypeById(long Id)
        {
           return await _context.EngagementTypes
            .FirstOrDefaultAsync(x => x.Id == Id && x.IsDeleted == false);
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