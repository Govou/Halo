using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaloBiz.Data;
using HaloBiz.Model.LAMS;
using HaloBiz.Repository.LAMS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HaloBiz.Repository.Impl.LAMS
{
    public class SBUToQuoteServiceProportionRepositoryImpl : ISBUToQuoteServiceProportionRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<SBUToQuoteServiceProportionRepositoryImpl> _logger;
        public SBUToQuoteServiceProportionRepositoryImpl(DataContext context, ILogger<SBUToQuoteServiceProportionRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }
         public async Task<SBUToQuoteServiceProportion> FindSBUToQuoteServiceProportionById(long Id)
        {
            return await _context.SBUToQuoteServiceProportions
                .FirstOrDefaultAsync( x => x.Id == Id && x.IsDeleted == false);
        }

        public async Task<SBUToQuoteServiceProportion> SaveSBUToQuoteServiceProportion(SBUToQuoteServiceProportion entity)
        {
            var sbuToQuoteProportion = await _context.SBUToQuoteServiceProportions.AddAsync(entity);
            if (await SaveChanges())
            {
                return sbuToQuoteProportion.Entity;
            }
            return null;            
        }

        public async Task<IEnumerable<SBUToQuoteServiceProportion>> SaveSBUToQuoteServiceProportion(IEnumerable<SBUToQuoteServiceProportion> entities)
        {
            if(entities.Count() == 0)
            {
                return null;
            }
            var quoteServiceId = entities.First().QuoteServiceId;
            await _context.SBUToQuoteServiceProportions.AddRangeAsync(entities);
            if (await SaveChanges())
            {
                return await FindAllSBUToQuoteServiceProportionByQuoteServiceId(quoteServiceId);
            }
            return null;            
        }

        
        public async Task<IEnumerable<SBUToQuoteServiceProportion>> FindAllSBUToQuoteServiceProportionByQuoteServiceId(long quoteServiceId)
        {
            return await _context.SBUToQuoteServiceProportions
                .Include(x => x.StrategicBusinessUnit)
                .Include(x => x.UserInvolved)
                .Where(x => x.IsDeleted == false && x.QuoteServiceId == quoteServiceId)
                .ToListAsync();
        }

        public async Task<SBUToQuoteServiceProportion> UpdateSBUToQuoteServiceProportion(SBUToQuoteServiceProportion entity)
        {
            var sbuToQuoteProportion = _context.SBUToQuoteServiceProportions.Update(entity);
            if (await SaveChanges())
            {
                return sbuToQuoteProportion.Entity;
            }
            return null;
        }

        public async Task<IEnumerable<SBUToQuoteServiceProportion>> UpdateSBUToQuoteServiceProportion(IEnumerable<SBUToQuoteServiceProportion> entities)
        {
            _context.SBUToQuoteServiceProportions.UpdateRange(entities);
            if (await SaveChanges())
            {
                return entities;
            }
            return null;
        }

        public async Task<bool> DeleteSBUToQuoteServiceProportion(SBUToQuoteServiceProportion entity)
        {
            entity.IsDeleted = true;
            _context.SBUToQuoteServiceProportions.Update(entity);
            return await SaveChanges();
        }
        public async Task<bool> DeleteSBUToQuoteServiceProportion(IEnumerable<SBUToQuoteServiceProportion> entities)
        {
            foreach (var entity in entities)
            {
                entity.IsDeleted = true;
            }
            _context.SBUToQuoteServiceProportions.UpdateRange(entities);
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