using HalobizMigrations.Data;
using HalobizMigrations.Models.Armada;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository.Impl
{
    public class CommanderRegistrationRepositoryImpl:ICommanderRegistrationRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<CommanderRegistrationRepositoryImpl> _logger;
        public CommanderRegistrationRepositoryImpl(HalobizContext context, ILogger<CommanderRegistrationRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<bool> DeleteCommander(CommanderProfile commanderProfile)
        {
            commanderProfile.IsDeleted = true;
            _context.CommanderProfiles.Update(commanderProfile);
            return await SaveChanges();
        }

        public async Task<IEnumerable<CommanderProfile>> FindAllCommanders()
        {
            return await _context.CommanderProfiles.Where(ct => ct.IsDeleted == false)
                .Include(ct=>ct.AttachedBranch)
                .Include(ct=>ct.AttachedOffice).Include(ct=>ct.Profile)
                .Include(ct=>ct.Rank).Include(ct=>ct.CommanderType)
                           .ToListAsync();
        }

        public async Task<CommanderProfile> FindCommanderById(long Id)
        {
            return await _context.CommanderProfiles.Include(ct => ct.AttachedBranch)
                .Include(ct => ct.AttachedOffice).Include(ct => ct.Profile)
                .Include(ct => ct.Rank).Include(ct => ct.CommanderType)
                           .FirstOrDefaultAsync(ct => ct.Id == Id && ct.IsDeleted == false);
        }

        public async Task<CommanderProfile> SaveCommander(CommanderProfile commanderProfile)
        {
            var savedEntity = await _context.CommanderProfiles.AddAsync(commanderProfile);
            if (await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<CommanderProfile> UpdateCommander(CommanderProfile commanderProfile)
        {
            var updatedEntity = _context.CommanderProfiles.Update(commanderProfile);
            if (await SaveChanges())
            {
                return updatedEntity.Entity;
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
