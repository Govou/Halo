using HalobizMigrations.Data;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HalobizMigrations.Models;
using Microsoft.EntityFrameworkCore;


namespace HaloBiz.Repository.Impl
{
    public class SbuproportionRepositoryImpl : ISbuproportionRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<SbuproportionRepositoryImpl> _logger;
        public SbuproportionRepositoryImpl(HalobizContext context, ILogger<SbuproportionRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<Sbuproportion> SaveSbuproportion(Sbuproportion sbuProportion)
        {
            var savedEntity = await _context.Sbuproportions.AddAsync(sbuProportion);
            if(await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<Sbuproportion> FindSbuproportionById(long Id)
        {
            return await _context.Sbuproportions
                .FirstOrDefaultAsync( x => x.Id == Id && x.IsDeleted == false);
        }
        public async Task<Sbuproportion> FindSbuproportionByOperatingEntityId(long Id)
        {
            return await _context.Sbuproportions
                .FirstOrDefaultAsync( x => x.OperatingEntityId == Id && x.IsDeleted == false);
        }

        public async Task<IEnumerable<Sbuproportion>> FindAllSbuproportions()
        {
            return await _context.Sbuproportions.Where(x => x.IsDeleted == false)
                .ToListAsync();
        }

        public async Task<Sbuproportion> UpdateSbuproportion(Sbuproportion sbuProportion)
        {
            var updatedEntity =  _context.Sbuproportions.Update(sbuProportion);
            if(await SaveChanges())
            {
                return updatedEntity.Entity;
            }
            return null;
        }

        public async Task<bool> DeleteSbuproportion(Sbuproportion sbuProportion)
        {
             sbuProportion.IsDeleted = true;
            _context.Sbuproportions.Update(sbuProportion);
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