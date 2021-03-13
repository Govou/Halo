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
    public class RegionRepositoryImpl : IRegionRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<RegionRepositoryImpl> _logger;
        public RegionRepositoryImpl(DataContext context, ILogger<RegionRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<Region> SaveRegion(Region region)
        {
            var savedEntity = await _context.Regions.AddAsync(region);
            if(await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<Region> FindRegionById(long Id)
        {
            return await _context.Regions
                .FirstOrDefaultAsync( region => region.Id == Id && region.IsDeleted == false);
        }

        public async Task<Region> FindRegionByName(string name)
        {
            return await _context.Regions
                .FirstOrDefaultAsync( region => region.Name == name && region.IsDeleted == false);
        }

        public async Task<IEnumerable<Region>> FindAllRegions()
        {
            return await _context.Regions.Where(region => region.IsDeleted == false)
                .ToListAsync();
        }

        public async Task<Region> UpdateRegion(Region region)
        {
            var updatedEntity =  _context.Regions.Update(region);
            if(await SaveChanges())
            {
                return updatedEntity.Entity;
            }
            return null;
        }

        public async Task<bool> DeleteRegion(Region region)
        {
             region.IsDeleted = true;
            _context.Regions.Update(region);
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