using HalobizMigrations.Data;
using HalobizMigrations.Models;
using HalobizMigrations.Models.Armada;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository.Impl
{
    public class ArmedEscortRegistrationRepositoryImpl:IArmedEscortRegistrationRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<ArmedEscortRegistrationRepositoryImpl> _logger;
        public ArmedEscortRegistrationRepositoryImpl(HalobizContext context, ILogger<ArmedEscortRegistrationRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<bool> DeleteArmedEscort(ArmedEscortProfile armedEscortProfile)
        {
            armedEscortProfile.IsDeleted = true;
            _context.ArmedEscortProfiles.Update(armedEscortProfile);
            return await SaveChanges();
        }

        public async Task<bool> DeleteArmedEscortTie(ArmedEscortSMORoutesResourceTie armedEscortTie)
        {
            armedEscortTie.IsDeleted = true;
            _context.ArmedEscortSMORoutesResourceTies.Update(armedEscortTie);
            return await SaveChanges();
        }

        public async Task<IEnumerable<ArmedEscortProfile>> FindAllArmedEscorts()
        {
            return await _context.ArmedEscortProfiles.Where(ae => ae.IsDeleted == false)
                .Include(ae=>ae.ArmedEscortType).Include(ae=>ae.Rank)
                .Include(ae=>ae.SupplierService).Include(ae=>ae.ServiceAssignment)
                          .ToListAsync();
        }

        public async Task<IEnumerable<ArmedEscortSMORoutesResourceTie>> FindAllArmedEscortTies()
        {
            return await _context.ArmedEscortSMORoutesResourceTies.Where(ae => ae.IsDeleted == false)
               .Include(ae => ae.SMORegion).Include(ae=>ae.CreatedBy)
               .Include(ae => ae.SMORoute).Include(ae => ae.Resource).Include(s=>s.Resource.ArmedEscortType)
                         .ToListAsync();
        }

        public async Task<ArmedEscortProfile> FindArmedEscortById(long Id)
        {
            return await _context.ArmedEscortProfiles.Include(ae => ae.ArmedEscortType).Include(ae => ae.Rank)
                .Include(ae => ae.SupplierService).Include(ae => ae.ServiceAssignment)
                .FirstOrDefaultAsync(ae => ae.Id == Id && ae.IsDeleted == false);
        }

        public async Task<ArmedEscortSMORoutesResourceTie> FindArmedEscortTieById(long Id)
        {
            return await _context.ArmedEscortSMORoutesResourceTies.Include(ae => ae.SMORegion).Include(ae => ae.CreatedBy)
               .Include(ae => ae.SMORoute).Include(ae => ae.Resource).Include(s => s.Resource.ArmedEscortType)
               .FirstOrDefaultAsync(ae => ae.Id == Id && ae.IsDeleted == false);
        }

        public async Task<IEnumerable<ArmedEscortSMORoutesResourceTie>> FindArmedEscortTieByResourceId(long resourceId)
        {
            return await _context.ArmedEscortSMORoutesResourceTies.Where(ae => ae.IsDeleted == false && ae.ResourceId == resourceId)
              .Include(ae => ae.SMORegion).Include(ae => ae.CreatedBy)
              .Include(ae => ae.SMORoute).Include(ae => ae.Resource).Include(s => s.Resource.ArmedEscortType)
                        .ToListAsync();
        }

        //public async Task<ArmedEscortSMORoutesResourceTie> FindArmedEscortTieByResourceId(long resourceId)
        //{
        //    return await _context.ArmedEscortSMORoutesResourceTies.Include(ae => ae.SMORegion).Include(ae => ae.CreatedBy)
        //       .Include(ae => ae.SMORoute).Include(ae => ae.Resource).Include(s => s.Resource.ArmedEscortType)
        //       .FirstOrDefaultAsync(ae => ae.ResourceId == resourceId && ae.IsDeleted == false);
        //}

        public ArmedEscortSMORoutesResourceTie GetServiceRegIdRegionAndRoute(long regRessourceId, long? RouteId)
        {
            return _context.ArmedEscortSMORoutesResourceTies.Where
                (ct => ct.ResourceId == regRessourceId && ct.SMORouteId == RouteId  && ct.IsDeleted == false).FirstOrDefault();
        }

        public ArmedEscortSMORoutesResourceTie GetServiceRegIdRegionAndRoute2(long? regRessourceId, long? RouteId)
        {
            return _context.ArmedEscortSMORoutesResourceTies.Where
               (ct => ct.ResourceId == regRessourceId && ct.SMORouteId == RouteId && ct.IsDeleted == false).FirstOrDefault();
        }

        public async Task<ArmedEscortProfile> SaveArmedEscort(ArmedEscortProfile armedEscortProfile)
        {
            var savedEntity = await _context.ArmedEscortProfiles.AddAsync(armedEscortProfile);
            if (await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<ArmedEscortSMORoutesResourceTie> SaveArmedEscortTie(ArmedEscortSMORoutesResourceTie armedEscortTie)
        {
            var savedEntity = await _context.ArmedEscortSMORoutesResourceTies.AddAsync(armedEscortTie);
            if (await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<ArmedEscortProfile> UpdateArmedEscort(ArmedEscortProfile armedEscortProfile)
        {
            var updatedEntity = _context.ArmedEscortProfiles.Update(armedEscortProfile);
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
