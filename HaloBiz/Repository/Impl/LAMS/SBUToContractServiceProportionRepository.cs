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
    public interface ISBUToContractServiceProportionRepository
    {
        Task<SbutoContractServiceProportion> FindSbutoContractServiceProportionById(long Id);
        Task<SbutoContractServiceProportion> SaveSbutoContractServiceProportion(SbutoContractServiceProportion entity);
        Task<IEnumerable<SbutoContractServiceProportion>> FindAllSbutoContractServiceProportionByQuoteServiceId(long quoteServiceId);
        Task<SbutoContractServiceProportion> UpdateSbutoContractServiceProportion(SbutoContractServiceProportion entity);
        Task<bool> DeleteSbutoContractServiceProportion(SbutoContractServiceProportion entity);
        Task<IEnumerable<SbutoContractServiceProportion>> SaveSbutoContractServiceProportion(IEnumerable<SbutoContractServiceProportion> entities);
        Task<bool> DeleteSbutoContractServiceProportion(IEnumerable<SbutoContractServiceProportion> entities);
        Task<IEnumerable<SbutoContractServiceProportion>> UpdateSbutoContractServiceProportion(IEnumerable<SbutoContractServiceProportion> entities);
    }

    public class SBUToContractServiceProportionRepository : ISBUToContractServiceProportionRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<SBUToContractServiceProportionRepository> _logger;
        public SBUToContractServiceProportionRepository(HalobizContext context, ILogger<SBUToContractServiceProportionRepository> logger)
        {
            this._logger = logger;
            this._context = context;
        }
         public async Task<SbutoContractServiceProportion> FindSbutoContractServiceProportionById(long Id)
        {
            return await _context.SbutoContractServiceProportions
                .FirstOrDefaultAsync( x => x.Id == Id && x.IsDeleted == false);
        }

        public async Task<SbutoContractServiceProportion> SaveSbutoContractServiceProportion(SbutoContractServiceProportion entity)
        {
            var SbutoContractProportion = await _context.SbutoContractServiceProportions.AddAsync(entity);
            if (await SaveChanges())
            {
                return SbutoContractProportion.Entity;
            }
            return null;            
        }

        public async Task<IEnumerable<SbutoContractServiceProportion>> SaveSbutoContractServiceProportion(IEnumerable<SbutoContractServiceProportion> entities)
        {
            if(entities.Count() == 0)
            {
                return null;
            }

            //var quoteServiceId = entities.First().QuoteServiceId;
            await _context.SbutoContractServiceProportions.AddRangeAsync(entities);
            await _context.SaveChangesAsync();

            return entities;        
        }

        
        public async Task<IEnumerable<SbutoContractServiceProportion>> FindAllSbutoContractServiceProportionByQuoteServiceId(long quoteServiceId)
        {
            return await _context.SbutoContractServiceProportions
                .Include(x => x.StrategicBusinessUnit)
                .Include(x => x.UserInvolved)
                .Where(x => x.IsDeleted == false && x.ContractServiceId == quoteServiceId)
                .ToListAsync();
        }

        public async Task<SbutoContractServiceProportion> UpdateSbutoContractServiceProportion(SbutoContractServiceProportion entity)
        {
            var SbutoContractProportion = _context.SbutoContractServiceProportions.Update(entity);
            if (await SaveChanges())
            {
                return SbutoContractProportion.Entity;
            }
            return null;
        }

        public async Task<IEnumerable<SbutoContractServiceProportion>> UpdateSbutoContractServiceProportion(IEnumerable<SbutoContractServiceProportion> entities)
        {
            _context.SbutoContractServiceProportions.UpdateRange(entities);
            if (await SaveChanges())
            {
                return entities;
            }
            return null;
        }

        public async Task<bool> DeleteSbutoContractServiceProportion(SbutoContractServiceProportion entity)
        {
            entity.IsDeleted = true;
            _context.SbutoContractServiceProportions.Update(entity);
            return await SaveChanges();
        }
        public async Task<bool> DeleteSbutoContractServiceProportion(IEnumerable<SbutoContractServiceProportion> entities)
        {
            foreach (var entity in entities)
            {
                entity.IsDeleted = true;
            }
            _context.SbutoContractServiceProportions.UpdateRange(entities);
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