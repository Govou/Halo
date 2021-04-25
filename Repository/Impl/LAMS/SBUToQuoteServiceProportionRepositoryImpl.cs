using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HalobizMigrations.Data;

using HaloBiz.Repository.LAMS;
using HalobizMigrations.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HaloBiz.Repository.Impl.LAMS
{
    public class SbutoQuoteServiceProportionRepositoryImpl : ISbutoQuoteServiceProportionRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<SbutoQuoteServiceProportionRepositoryImpl> _logger;
        public SbutoQuoteServiceProportionRepositoryImpl(HalobizContext context, ILogger<SbutoQuoteServiceProportionRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }
         public async Task<SbutoQuoteServiceProportion> FindSbutoQuoteServiceProportionById(long Id)
        {
            return await _context.SbutoQuoteServiceProportions
                .FirstOrDefaultAsync( x => x.Id == Id && x.IsDeleted == false);
        }

        public async Task<SbutoQuoteServiceProportion> SaveSbutoQuoteServiceProportion(SbutoQuoteServiceProportion entity)
        {
            var sbuToQuoteProportion = await _context.SbutoQuoteServiceProportions.AddAsync(entity);
            if (await SaveChanges())
            {
                return sbuToQuoteProportion.Entity;
            }
            return null;            
        }

        public async Task<IEnumerable<SbutoQuoteServiceProportion>> SaveSbutoQuoteServiceProportion(IEnumerable<SbutoQuoteServiceProportion> entities)
        {
            if(entities.Count() == 0)
            {
                return null;
            }
            var quoteServiceId = entities.First().QuoteServiceId;
            await _context.SbutoQuoteServiceProportions.AddRangeAsync(entities);
            if (await SaveChanges())
            {
                return await FindAllSbutoQuoteServiceProportionByQuoteServiceId(quoteServiceId);
            }
            return null;            
        }

        
        public async Task<IEnumerable<SbutoQuoteServiceProportion>> FindAllSbutoQuoteServiceProportionByQuoteServiceId(long quoteServiceId)
        {
            return await _context.SbutoQuoteServiceProportions
                .Include(x => x.StrategicBusinessUnit)
                .Include(x => x.UserInvolved)
                .Where(x => x.IsDeleted == false && x.QuoteServiceId == quoteServiceId)
                .ToListAsync();
        }

        public async Task<SbutoQuoteServiceProportion> UpdateSbutoQuoteServiceProportion(SbutoQuoteServiceProportion entity)
        {
            var sbuToQuoteProportion = _context.SbutoQuoteServiceProportions.Update(entity);
            if (await SaveChanges())
            {
                return sbuToQuoteProportion.Entity;
            }
            return null;
        }

        public async Task<IEnumerable<SbutoQuoteServiceProportion>> UpdateSbutoQuoteServiceProportion(IEnumerable<SbutoQuoteServiceProportion> entities)
        {
            _context.SbutoQuoteServiceProportions.UpdateRange(entities);
            if (await SaveChanges())
            {
                return entities;
            }
            return null;
        }

        public async Task<bool> DeleteSbutoQuoteServiceProportion(SbutoQuoteServiceProportion entity)
        {
            entity.IsDeleted = true;
            _context.SbutoQuoteServiceProportions.Update(entity);
            return await SaveChanges();
        }
        public async Task<bool> DeleteSbutoQuoteServiceProportion(IEnumerable<SbutoQuoteServiceProportion> entities)
        {
            foreach (var entity in entities)
            {
                entity.IsDeleted = true;
            }
            _context.SbutoQuoteServiceProportions.UpdateRange(entities);
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
    }
}