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
    public class SMORouteAndRegionRepositoryImpl:ISMORouteAndRegionRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<SMORouteAndRegionRepositoryImpl> _logger;
        public SMORouteAndRegionRepositoryImpl(HalobizContext context, ILogger<SMORouteAndRegionRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<SMORoute> SaveSMORoute(SMORoute sMORoute)
        {
            var savedEntity = await _context.SMORoutes.AddAsync(sMORoute);
            if (await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<SMORoute> FindSMORouteById(long Id)
        {
            return await _context.SMORoutes
                .FirstOrDefaultAsync(route => route.Id == Id);
        }

        public async Task<IEnumerable<SMORoute>> FindAllSMORoutes()
        {
            return await _context.SMORoutes
                .ToListAsync();
        }

        public async Task<SMORoute> UpdateSMORoute(SMORoute sMORoute)
        {
            var updatedEntity = _context.SMORoutes.Update(sMORoute);
            if (await SaveChanges())
            {
                return updatedEntity.Entity;
            }
            return null;
        }

        //Region
        public async Task<SMORegion> SaveSMORegion(SMORegion sMORegion)
        {
            var savedEntity = await _context.SMORegions.AddAsync(sMORegion);
            if (await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<SMORegion> FindSMORegionById(long Id)
        {
            return await _context.SMORegions
                .FirstOrDefaultAsync(region => region.Id == Id);
        }

        public async Task<IEnumerable<SMORegion>> FindAllSMORegions()
        {
            return await _context.SMORegions.Where(c=>c.IsDeleted == false).Include(region => region.CreatedBy)
                .Include(region => region.SMORoutes
                .Where(route => route.IsDeleted == false))
                .ToListAsync();
                
        }

        public async Task<SMORegion> UpdateSMORegion(SMORegion sMORegion)
        {
            var updatedEntity = _context.SMORegions.Update(sMORegion);
            if (await SaveChanges())
            {
                return updatedEntity.Entity;
            }
            return null;
        }

        //public async Task<bool> DeleteRegion(Region region)
        //{
        //    region.IsDeleted = true;
        //    _context.Regions.Update(region);
        //    return await SaveChanges();
        //}

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
