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
    public class PilotRegistrationRepositoryImpl : IPilotRegistrationRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<PilotRegistrationRepositoryImpl> _logger;
        public PilotRegistrationRepositoryImpl(HalobizContext context, ILogger<PilotRegistrationRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }
        public async Task<bool> DeletePilot(PilotProfile pilotProfile)
        {
            pilotProfile.IsDeleted = true;
            _context.PilotProfiles.Update(pilotProfile);
            return await SaveChanges();
        }

        public async Task<IEnumerable<PilotProfile>> FindAllPilots()
        {
            return await _context.PilotProfiles.Where(rank => rank.IsDeleted == false)
                                       .ToListAsync();
        }

        public async Task<PilotProfile> FindPilotById(long Id)
        {
            return await _context.PilotProfiles.FirstOrDefaultAsync(aer => aer.Id == Id && aer.IsDeleted == false);
        }

        public async Task<PilotProfile> SavePilot(PilotProfile pilotProfile)
        {
            var savedEntity = await _context.PilotProfiles.AddAsync(pilotProfile);
            if (await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<PilotProfile> UpdatePilot(PilotProfile pilotProfile)
        {
            var updatedEntity = _context.PilotProfiles.Update(pilotProfile);
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
