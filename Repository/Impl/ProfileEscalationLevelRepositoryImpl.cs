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
    public class ProfileEscalationLevelRepositoryImpl : IProfileEscalationLevelRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<ProfileEscalationLevelRepositoryImpl> _logger;
        public ProfileEscalationLevelRepositoryImpl(HalobizContext context, ILogger<ProfileEscalationLevelRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<bool> DeleteProfileEscalationLevel(ProfileEscalationLevel profileEscalationLevel)
        {
            profileEscalationLevel.IsDeleted = true;
            _context.ProfileEscalationLevels.Update(profileEscalationLevel);
            return await SaveChanges();
        }

        public async Task<IEnumerable<ProfileEscalationLevel>> FindAllProfileEscalationLevels()
        {
            return await _context.ProfileEscalationLevels
               .Where(profileEscalationLevel => profileEscalationLevel.IsDeleted == false)
               .OrderBy(profileEscalationLevel => profileEscalationLevel.CreatedAt)
               .ToListAsync();
        }

        public async Task<ProfileEscalationLevel> FindProfileEscalationLevelById(long Id)
        {
            return await _context.ProfileEscalationLevels
                .Where(profileEscalationLevel => profileEscalationLevel.IsDeleted == false)
                .FirstOrDefaultAsync(profileEscalationLevel => profileEscalationLevel.Id == Id && profileEscalationLevel.IsDeleted == false);

        }

        /*public async Task<ProfileEscalationLevel> FindProfileEscalationLevelByName(string name)
        {
            return await _context.ProfileEscalationLevels
                 .Where(profileEscalationLevel => profileEscalationLevel.IsDeleted == false)
                 .FirstOrDefaultAsync(profileEscalationLevel => profileEscalationLevel.Caption == name && profileEscalationLevel.IsDeleted == false);

        }*/

        public async Task<ProfileEscalationLevel> SaveProfileEscalationLevel(ProfileEscalationLevel profileEscalationLevel)
        {
            var profileEscalationLevelEntity = await _context.ProfileEscalationLevels.AddAsync(profileEscalationLevel);
            if (await SaveChanges())
            {
                return profileEscalationLevelEntity.Entity;
            }
            return null;
        }

        public async Task<ProfileEscalationLevel> UpdateProfileEscalationLevel(ProfileEscalationLevel profileEscalationLevel)
        {
            var profileEscalationLevelEntity = _context.ProfileEscalationLevels.Update(profileEscalationLevel);
            if (await SaveChanges())
            {
                return profileEscalationLevelEntity.Entity;
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
