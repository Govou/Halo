using HalobizMigrations.Data;
using HalobizMigrations.Models;
using HalobizMigrations.Models.Complaints;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository.Impl
{
    public class EscalationLevelRepositoryImpl : IEscalationLevelRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<EscalationLevelRepositoryImpl> _logger;
        public EscalationLevelRepositoryImpl(HalobizContext context, ILogger<EscalationLevelRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<bool> DeleteEscalationLevel(EscalationLevel escalationLevel)
        {
            escalationLevel.IsDeleted = true;
            _context.EscalationLevels.Update(escalationLevel);
            return await SaveChanges();
        }

        public async Task<IEnumerable<EscalationLevel>> FindAllEscalationLevels()
        {
            return await _context.EscalationLevels
               .Where(escalationLevel => escalationLevel.IsDeleted == false)
               .OrderBy(escalationLevel => escalationLevel.CreatedAt)
               .ToListAsync();
        }

        public async Task<EscalationLevel> FindEscalationLevelById(long Id)
        {
            return await _context.EscalationLevels
                .Where(escalationLevel => escalationLevel.IsDeleted == false)
                .FirstOrDefaultAsync(escalationLevel => escalationLevel.Id == Id && escalationLevel.IsDeleted == false);

        }

        public async Task<EscalationLevel> FindEscalationLevelByName(string name)
        {
            return await _context.EscalationLevels
                 .Where(escalationLevel => escalationLevel.IsDeleted == false)
                 .FirstOrDefaultAsync(escalationLevel => escalationLevel.Caption == name && escalationLevel.IsDeleted == false);

        }

        public async Task<EscalationLevel> SaveEscalationLevel(EscalationLevel escalationLevel)
        {
            var escalationLevelEntity = await _context.EscalationLevels.AddAsync(escalationLevel);
            if (await SaveChanges())
            {
                return escalationLevelEntity.Entity;
            }
            return null;
        }

        public async Task<EscalationLevel> UpdateEscalationLevel(EscalationLevel escalationLevel)
        {
            var escalationLevelEntity = _context.EscalationLevels.Update(escalationLevel);
            if (await SaveChanges())
            {
                return escalationLevelEntity.Entity;
            }
            return null;
        }

        private async Task<bool> SaveChanges()
        {
            try
            {
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }
    }
}
