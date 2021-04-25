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
    public class EngagementReasonRepositoryImpl : IEngagementReasonRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<EngagementReasonRepositoryImpl> _logger;
        public EngagementReasonRepositoryImpl(HalobizContext context, ILogger<EngagementReasonRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }
        public async Task<EngagementReason> SaveEngagementReason(EngagementReason engagementReason)
        {
            var engagementReasonEntity = await _context.EngagementReasons.AddAsync(engagementReason);
            if(await SaveChanges())
            {
                return engagementReasonEntity.Entity;
            }
            return null;
        }
        public async Task<IEnumerable<EngagementReason>> GetEngagementReasons()
        {
            return await _context.EngagementReasons
                .Where(x => x.IsDeleted == false)
                .OrderBy(x => x.Caption)
                .ToListAsync();
        }

        public async Task<EngagementReason> UpdateEngagementReason(EngagementReason engagementReason)
        {
             var engagementReasonEntity =  _context.EngagementReasons.Update(engagementReason);
            if(await SaveChanges())
            {
                return engagementReasonEntity.Entity;
            }
            return null;
        }
        public async Task<bool> DeleteEngagementReason(EngagementReason engagementReason)
        {
            engagementReason.IsDeleted = true;
            _context.EngagementReasons.Update(engagementReason);
            return await SaveChanges();
        }
        public async Task<EngagementReason> FindEngagementReasonById(long Id)
        {
           return await _context.EngagementReasons
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