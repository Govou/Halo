using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaloBiz.Data;
using HaloBiz.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HaloBiz.Repository.Impl
{
    public class ZoneRepositoryImpl : IZoneRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<ZoneRepositoryImpl> _logger;
        public ZoneRepositoryImpl(DataContext context, ILogger<ZoneRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<Zone> SaveZone(Zone zone)
        {
            var savedEntity = await _context.Zones.AddAsync(zone);
            if(await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<Zone> FindZoneById(long Id)
        {
            return await _context.Zones
                .FirstOrDefaultAsync( zone => zone.Id == Id && zone.IsDeleted == false);
        }

        public async Task<Zone> FindZoneByName(string name)
        {
            return await _context.Zones
                .FirstOrDefaultAsync( zone => zone.Name == name && zone.IsDeleted == false);
        }

        public async Task<IEnumerable<Zone>> FindAllZones()
        {
            return await _context.Zones.Where(zone => zone.IsDeleted == false)
                .ToListAsync();
        }

        public async Task<Zone> UpdateZone(Zone zone)
        {
            var updatedEntity =  _context.Zones.Update(zone);
            if(await SaveChanges())
            {
                return updatedEntity.Entity;
            }
            return null;
        }

        public async Task<bool> DeleteZone(Zone zone)
        {
             zone.IsDeleted = true;
            _context.Zones.Update(zone);
            return await SaveChanges();
        }

        private async Task<bool> SaveChanges()
        {
           try{
               return  await _context.SaveChangesAsync() > 0;
           }catch(Exception ex)
           {
               _logger.LogError(ex.Message);
               return false;
           }
        }
        
    }
}