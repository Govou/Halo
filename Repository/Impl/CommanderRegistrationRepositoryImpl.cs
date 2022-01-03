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

        public async Task<bool> DeleteCommanderTie(CommanderSMORoutesResourceTie commanderProfileTie)
        {
            commanderProfileTie.IsDeleted = true;
            _context.CommanderSMORoutesResourceTies.Update(commanderProfileTie);
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

        public async Task<IEnumerable<CommanderSMORoutesResourceTie>> FindAllCommanderTies()
        {
            return await _context.CommanderSMORoutesResourceTies.Where(ct => ct.IsDeleted == false)
               .Include(ct => ct.Resource)
               .Include(ct => ct.Resource.AttachedBranch).Include(ct => ct.Resource.AttachedOffice)
               .Include(ct => ct.SMORegion).Include(ct => ct.Resource.CommanderType).Include(s=>s.SMORoute)
               .Include(s=>s.Resource.Profile)
                          .ToListAsync();
        }

        public async Task<IEnumerable<CommanderSMORoutesResourceTie>> FindAllCommanderTiesByResourceId(long resourceId)
        {
            return await _context.CommanderSMORoutesResourceTies.Where(ct => ct.IsDeleted == false && ct.ResourceId == resourceId)
              .Include(ct => ct.Resource)
              .Include(ct => ct.Resource.AttachedBranch).Include(ct => ct.Resource.AttachedOffice)
              .Include(ct => ct.SMORegion).Include(ct => ct.Resource.CommanderType).Include(s => s.SMORoute)
              .Include(s => s.Resource.Profile)
                         .ToListAsync();
        }

        public async Task<CommanderProfile> FindCommanderById(long Id)  
        {
            return await _context.CommanderProfiles.Include(ct => ct.AttachedBranch)
                .Include(ct => ct.AttachedOffice).Include(ct => ct.Profile)
                .Include(ct => ct.Rank).Include(ct => ct.CommanderType)
                           .FirstOrDefaultAsync(ct => ct.Id == Id && ct.IsDeleted == false);
        }

        public async Task<CommanderSMORoutesResourceTie> FindCommanderTieById(long Id)
        {
            return await _context.CommanderSMORoutesResourceTies.Include(ct => ct.Resource)
               .Include(ct => ct.Resource.AttachedBranch).Include(ct => ct.Resource.AttachedOffice).Include(s => s.Resource.Profile)
               .Include(ct => ct.SMORegion).Include(ct => ct.Resource.CommanderType).Include(s => s.SMORoute)
                          .FirstOrDefaultAsync(ct => ct.Id == Id && ct.IsDeleted == false);
        }

        public async Task<CommanderSMORoutesResourceTie> FindCommanderTieByResourceId(long resourceId)
        {
            return await _context.CommanderSMORoutesResourceTies.Include(ct => ct.Resource)
              .Include(ct => ct.Resource.AttachedBranch).Include(ct => ct.Resource.AttachedOffice).Include(s => s.Resource.Profile)
              .Include(ct => ct.SMORegion).Include(ct => ct.Resource.CommanderType).Include(s => s.SMORoute)
                         .FirstOrDefaultAsync(ct => ct.ResourceId == resourceId && ct.IsDeleted == false);
        }

        public CommanderProfile FindCommanderUserProfileById(long profileId)  //FindCommanderUserProfileById
        {
            return  _context.CommanderProfiles
                           .Where(ct => ct.ProfileId == profileId && ct.IsDeleted == false).FirstOrDefault();
        }

        //public CommanderSMORoutesResourceTie FindCommanderUserProfileTieById(long Id)
        //{
        //    throw new NotImplementedException();
        //}

        public CommanderSMORoutesResourceTie GetResourceRegIdRegionAndRouteId(long regRessourceId, long? RouteId, long? RegionId)
        {
            return _context.CommanderSMORoutesResourceTies.Where
               (ct => ct.ResourceId == regRessourceId && ct.SMORouteId == RouteId && ct.SMORegionId == RegionId && ct.IsDeleted == false).FirstOrDefault();
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

        public async Task<CommanderSMORoutesResourceTie> SaveCommanderTie(CommanderSMORoutesResourceTie commanderProfileTie)
        {
            var savedEntity = await _context.CommanderSMORoutesResourceTies.AddAsync(commanderProfileTie);
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
