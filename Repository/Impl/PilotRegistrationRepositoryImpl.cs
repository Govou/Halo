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

        public async Task<bool> DeletePilotTie(PilotSMORoutesResourceTie pilotProfileTie)
        {
            pilotProfileTie.IsDeleted = true;
            _context.PilotSMORoutesResourceTies.Update(pilotProfileTie);
            return await SaveChanges();
        }

        public async Task<IEnumerable<PilotProfile>> FindAllPilots()
        {
            return await _context.PilotProfiles.Where(pi => pi.IsDeleted == false)
                .Include(pi=>pi.MeansOfIdentification).Include(pi=>pi.PilotType)
                .Include(pi=>pi.Rank).Include(pi=>pi.CreatedBy)
                                       .ToListAsync();
        }

        public async Task<IEnumerable<PilotSMORoutesResourceTie>> FindAllPilotTies()
        {
            return await _context.PilotSMORoutesResourceTies.Where(pi => pi.IsDeleted == false)
               .Include(pi => pi.SMORegion).Include(pi => pi.SMORoute).Include(s => s.Resource.PilotType)
               .Include(pi => pi.Resource).Include(pi => pi.CreatedBy)
                                      .ToListAsync();
        }

        public async Task<PilotProfile> FindPilotById(long Id)
        {
            return await _context.PilotProfiles.Include(pi => pi.MeansOfIdentification).Include(pi => pi.PilotType)
                .Include(pi => pi.Rank).Include(pi => pi.CreatedBy).FirstOrDefaultAsync(aer => aer.Id == Id && aer.IsDeleted == false);
        }

        public async Task<PilotSMORoutesResourceTie> FindPilotTieById(long Id)
        {
            return await _context.PilotSMORoutesResourceTies.Include(pi => pi.SMORegion).Include(pi => pi.SMORoute).Include(s => s.Resource.PilotType)
               .Include(pi => pi.Resource).Include(pi => pi.CreatedBy).FirstOrDefaultAsync(aer => aer.Id == Id && aer.IsDeleted == false);
        }

        public async Task<IEnumerable<PilotSMORoutesResourceTie>> FindPilotTieByResourceId(long resourceId)
        {
            return await _context.PilotSMORoutesResourceTies.Where(pi => pi.IsDeleted == false && pi.ResourceId == resourceId)
               .Include(pi => pi.SMORegion).Include(pi => pi.SMORoute).Include(s => s.Resource.PilotType)
               .Include(pi => pi.Resource).Include(pi => pi.CreatedBy)
                                      .ToListAsync();
        }

        //public async Task<PilotSMORoutesResourceTie> FindPilotTieByResourceId(long resourceId)
        //{
        //    return await _context.PilotSMORoutesResourceTies.Include(pi => pi.SMORegion).Include(pi => pi.SMORoute).Include(s => s.Resource.PilotType)
        //       .Include(pi => pi.Resource).Include(pi => pi.CreatedBy).FirstOrDefaultAsync(aer => aer.ResourceId == resourceId && aer.IsDeleted == false);
        //}

        public PilotSMORoutesResourceTie GetResourceRegIdRegionAndRouteId(long regRessourceId, long? RouteId, long? RegionId)
        {
            return _context.PilotSMORoutesResourceTies.Where
                (ct => ct.ResourceId == regRessourceId && ct.SMORouteId == RouteId && ct.SMORegionId == RegionId && ct.IsDeleted == false).FirstOrDefault();
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

        public async Task<PilotSMORoutesResourceTie> SavePilotTie(PilotSMORoutesResourceTie pilotProfileTie)
        {
            var savedEntity = await _context.PilotSMORoutesResourceTies.AddAsync(pilotProfileTie);
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
