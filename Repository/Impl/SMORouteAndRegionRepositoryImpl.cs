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
            return await _context.SMORoutes.Include(reg=>reg.SMORegion)
                .Include(r=>r.VehiclesOnRoute.Where(r=>r.IsDeleted == false))
                .Include(r=>r.ArmedEscortsOnRoute.Where(ae=>ae.IsDeleted == false)).
                Include(r=>r.PilotsOnRoute.Where(pi=>pi.IsDeleted == false))
                .FirstOrDefaultAsync(route => route.Id == Id && route.IsDeleted == false);
        }

        public async Task<IEnumerable<SMORoute>> FindAllSMORoutes()
        {
            return await _context.SMORoutes.Where(r=>r.IsDeleted == false).Include(r => r.VehiclesOnRoute.Where(r => r.IsDeleted == false))
                .Include(r => r.ArmedEscortsOnRoute.Where(ae => ae.IsDeleted == false)).
                Include(r => r.PilotsOnRoute.Where(pi => pi.IsDeleted == false))
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

        public async Task<bool> DeleteSMORoute(SMORoute sMORoute)
        {
            sMORoute.IsDeleted = true;
            _context.SMORoutes.Update(sMORoute);
            return await SaveChanges();
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
            return await _context.SMORegions.Include(r => r.SMORoutes.Where(route => route.IsDeleted == false))
                .Include(r => r.CreatedBy)
                .FirstOrDefaultAsync(region => region.Id == Id && region.IsDeleted == false);
        }

        public async Task<IEnumerable<SMORegion>> FindAllSMORegions()
        {
            return await _context.SMORegions.Where(r=>r.IsDeleted == false)
                .Include(r=>r.SMORoutes.Where(route=>route.IsDeleted == false))
                .Include(r=>r.CreatedBy)
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

        public async Task<bool> DeleteSMORegion(SMORegion sMORegion)
        {
            sMORegion.IsDeleted = true;
            _context.SMORegions.Update(sMORegion);
            return await SaveChanges();
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

        //Return Route
        public Task<SMOReturnRoute> SaveSMOReturnRoute(SMOReturnRoute sMOReturnRoute)
        {
            throw new NotImplementedException();
        }

        public Task<SMOReturnRoute> UpdateSMOReturnRoute(SMORoute sMOReturnRoute)
        {
            throw new NotImplementedException();
        }

        public Task<SMOReturnRoute> FindSMOReturnRouteById(long id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SMOReturnRoute>> FindAllSMOReturnRoutes()
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteSMOReturnRoute(SMOReturnRoute sMOReturnRoute)
        {
            throw new NotImplementedException();
        }
    }
}
