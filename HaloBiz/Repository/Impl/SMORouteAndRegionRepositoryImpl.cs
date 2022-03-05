using HaloBiz.DTOs.TransferDTOs;
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
            var returnRoute = new SMOReturnRoute();
            var savedEntity = await _context.SMORoutes.AddAsync(sMORoute);
            //if(sMORoute.IsReturnRouteRequired == true)
            //{
            //    var getId = sMORoute.Id;
            //    returnRoute.SMORouteId = getId;
            //    //returnRoute.ReturnRouteId = returnRoute.ReturnRouteId;
            //    //returnRoute.RRecoveryTime = returnRoute.RRecoveryTime;
            //    //var savedReturnRoute = await _context.SMOReturnRoutes.AddAsync(returnRoute);
            //}
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
                .Include(r => r.SMORegion)
                .FirstOrDefaultAsync(route => route.Id == Id && route.IsDeleted == false);
        }

        public async Task<IEnumerable<SMORoute>> FindAllSMORoutes()
        {
            return await _context.SMORoutes.Where(r=>r.IsDeleted == false).Include(r => r.VehiclesOnRoute.Where(r => r.IsDeleted == false))
                .Include(r => r.ArmedEscortsOnRoute.Where(ae => ae.IsDeleted == false)).
                Include(r => r.PilotsOnRoute.Where(pi => pi.IsDeleted == false))
                .Include(r=>r.SMORegion)
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

        //public async Task<IEnumerable<SMORouteRegionTransferDTO>> FindAllSMORouteRegions()
        //{
        //    return new SMORouteRegionTransferDTO()
        //    {
        //        SMORoutes = await _context.SMORoutes.Where(x => x.IsDeleted == false).ToListAsync(),
        //        SMORegions = await _context.SMORegions.Where(x => x.IsDeleted == false).ToListAsync()
        //    };

        //}

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
        public async Task<SMOReturnRoute> SaveSMOReturnRoute(SMOReturnRoute sMOReturnRoute)
        {
            var savedEntity = await _context.SMOReturnRoutes.AddAsync(sMOReturnRoute);
            if (await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<SMOReturnRoute> UpdateSMOReturnRoute(SMOReturnRoute sMOReturnRoute)
        {
            var updatedEntity = _context.SMOReturnRoutes.Update(sMOReturnRoute);
            if (await SaveChanges())
            {
                return updatedEntity.Entity;
            }
            return null;
        }

        public async Task<SMOReturnRoute> FindSMOReturnRouteById(long id)
        {
            return await _context.SMOReturnRoutes.Include(r => r.SMORoute)
                           .Include(r => r.CreatedBy).Include(r=>r.ReturnRoute)
                           .FirstOrDefaultAsync(r => r.Id == id && r.IsDeleted == false && r.SMORoute.IsReturnRouteRequired == true);
        }

        public async Task<IEnumerable<SMOReturnRoute>> FindAllSMOReturnRoutes()
        {
            return await _context.SMOReturnRoutes.Where(r => r.IsDeleted == false && r.SMORoute.IsReturnRouteRequired == true)
                           .Include(r => r.SMORoute).Include(r => r.ReturnRoute)
                           .Include(r => r.CreatedBy)
                           .ToListAsync();
        }

        public async Task<bool> DeleteSMOReturnRoute(SMOReturnRoute sMOReturnRoute)
        {
            sMOReturnRoute.IsDeleted = true;
            _context.SMOReturnRoutes.Update(sMOReturnRoute);
            return await SaveChanges();
        }

        public SMORoute GetRouteName(string Name)
        {
            return _context.SMORoutes.Where(c => c.RouteName == Name && c.IsDeleted == false).FirstOrDefault();

        }

        public SMORegion GetRegionName(string Name)
        {
            return _context.SMORegions.Where(c => c.RegionName == Name && c.IsDeleted == false).FirstOrDefault();
        }

        public bool hasReturnRoute(long? id)
        {
            var hasRoute = _context.SMOReturnRoutes
                               .Where(ct =>  ct.SMORoute.IsReturnRouteRequired == true && ct.SMORoute.Id == id  ).FirstOrDefault();
            if (hasRoute !=null) return true;
            else
                return false;

            //return _context.SMOReturnRoutes
            //               .Where(ct => ct.Id == Id && ct.ReturnRoute.IsReturnRouteRequired == true && ct.IsDeleted == false).FirstOrDefault();

        }

        public async Task<IEnumerable<SMORoute>> FindAllRoutesWithReturnRoute()
        {
            return await _context.SMORoutes.Where(r => r.IsDeleted == false && r.IsReturnRouteRequired == true).Include(r => r.VehiclesOnRoute.Where(r => r.IsDeleted == false))
                           .Include(r => r.ArmedEscortsOnRoute.Where(ae => ae.IsDeleted == false)).
                           Include(r => r.PilotsOnRoute.Where(pi => pi.IsDeleted == false))
                           .Include(r => r.SMORegion)
                           .ToListAsync();
        }

        public SMOReturnRoute GetSMORouteId(long? routeId)
        {
            return _context.SMOReturnRoutes
                                      .Where(ct => ct.SMORouteId == routeId && ct.IsDeleted == false).FirstOrDefault();
        }

        public async Task<IEnumerable<SMORoute>> FindAllSMORoutesByName(string routeName)
        {
            return await _context.SMORoutes.Where(r => r.IsDeleted == false && r.RouteName == routeName).Include(r => r.VehiclesOnRoute.Where(r => r.IsDeleted == false))
              .Include(r => r.ArmedEscortsOnRoute.Where(ae => ae.IsDeleted == false)).
              Include(r => r.PilotsOnRoute.Where(pi => pi.IsDeleted == false))
              .Include(r => r.SMORegion)
              .ToListAsync();
        }

        //RouteMap
        public async Task<SMORouteAndStateMap> SaveSMORouteMap(SMORouteAndStateMap sMOMapRoute)
        {
            var savedEntity = await _context.SMORouteAndStateMaps.AddAsync(sMOMapRoute);
            if (await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public Task<SMOReturnRoute> UpdateSMORouteMap(SMORouteAndStateMap sMOMapRoute)
        {
            throw new NotImplementedException();
        }

        public async Task<SMORouteAndStateMap> FindSMORouteMapById(long id)
        {
            return await _context.SMORouteAndStateMaps.Include(r => r.SMORegion).Include(x=>x.SMORoute)
                 .Include(r => r.CreatedBy).Include(x=>x.State)
                 .FirstOrDefaultAsync(route => route.Id == id );
        }

        public SMORouteAndStateMap GetSMORouteMapId(long? routeId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<SMORouteAndStateMap>> FindAllSMORoutesMap()
        {
            return await _context.SMORouteAndStateMaps.Where(r => r.IsDeleted == false).Include(r => r.State)
            .Include(r => r.SMORegion).
            Include(r => r.SMORoute)
            .ToListAsync();
        }

        public async Task<bool> DeleteSMORouteMap(SMORouteAndStateMap sMOMapRoute)
        {
            sMOMapRoute.IsDeleted = true;
            _context.SMORouteAndStateMaps.Update(sMOMapRoute);
            return await SaveChanges();
        }

        public  SMORouteAndStateMap GetSMORouteMapByRouteIdAndStateId(long? routeId, long? stateId)
        {
            return  _context.SMORouteAndStateMaps.Where
             (ct => ct.SMORouteId == routeId && ct.StateId == stateId && ct.IsDeleted == false).FirstOrDefault();
        }

        public async Task<IEnumerable<SMORouteAndStateMap>> FindAllRouteMapsByRouteId(long routeId)
        {
            return await _context.SMORouteAndStateMaps.Where(r => r.IsDeleted == false && r.SMORouteId == routeId).Include(r => r.State)
            .Include(r => r.SMORegion).
            Include(r => r.SMORoute)
            .ToListAsync();
        }
    }
}
