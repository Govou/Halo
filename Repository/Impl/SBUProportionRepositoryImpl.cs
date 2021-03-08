using HaloBiz.Data;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaloBiz.Model;
using Microsoft.EntityFrameworkCore;


namespace HaloBiz.Repository.Impl
{
    public class SBUProportionRepositoryImpl : ISBUProportionRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<SBUProportionRepositoryImpl> _logger;
        public SBUProportionRepositoryImpl(DataContext context, ILogger<SBUProportionRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<SBUProportion> SaveSBUProportion(SBUProportion sbuProportion)
        {
            var savedEntity = await _context.SBUProportions.AddAsync(sbuProportion);
            if(await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<SBUProportion> FindSBUProportionById(long Id)
        {
            return await _context.SBUProportions
                .FirstOrDefaultAsync( x => x.Id == Id && x.IsDeleted == false);
        }
        public async Task<SBUProportion> FindSBUProportionByOperatingEntityId(long Id)
        {
            return await _context.SBUProportions
                .FirstOrDefaultAsync( x => x.OperatingEntityId == Id && x.IsDeleted == false);
        }

        public async Task<IEnumerable<SBUProportion>> FindAllSBUProportions()
        {
            return await _context.SBUProportions.Where(x => x.IsDeleted == false)
                .ToListAsync();
        }

        public async Task<SBUProportion> UpdateSBUProportion(SBUProportion sbuProportion)
        {
            var updatedEntity =  _context.SBUProportions.Update(sbuProportion);
            if(await SaveChanges())
            {
                return updatedEntity.Entity;
            }
            return null;
        }

        public async Task<bool> DeleteSBUProportion(SBUProportion sbuProportion)
        {
             sbuProportion.IsDeleted = true;
            _context.SBUProportions.Update(sbuProportion);
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