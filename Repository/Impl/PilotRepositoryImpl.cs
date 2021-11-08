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
    public class PilotRepositoryImpl : IPilotRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<PilotRepositoryImpl> _logger;
        public PilotRepositoryImpl(HalobizContext context, ILogger<PilotRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }
        public async Task<bool> DeletePilotRank(PilotRank pilotRank)
        {
            pilotRank.IsDeleted = true;
            _context.PilotRanks.Update(pilotRank);
            return await SaveChanges();
        }

        public async Task<bool> DeletePilotType(PilotType pilotType)
        {
            pilotType.IsDeleted = true;
            _context.PilotTypes.Update(pilotType);
            return await SaveChanges();
        }

        public async Task<IEnumerable<PilotRank>> FindAllPilotRanks()
        {
            return await _context.PilotRanks.Where(rank => rank.IsDeleted == false)
                            .ToListAsync();
        }

        public async Task<IEnumerable<PilotType>> FindAllPilotTypes()
        {
            return await _context.PilotTypes.Where(r => r.IsDeleted == false)
                                        .ToListAsync();
        }

        public async Task<PilotRank> FindPilotRankById(long Id)
        {
            return await _context.PilotRanks.FirstOrDefaultAsync(aer => aer.Id == Id && aer.IsDeleted == false);
        }

        public async Task<PilotType> FindPilotTypeById(long Id)
        {
            return await _context.PilotTypes.FirstOrDefaultAsync(aer => aer.Id == Id && aer.IsDeleted == false);
        }

        public async Task<PilotRank> SavePilotRank(PilotRank pilotRank)
        {
            var savedEntity = await _context.PilotRanks.AddAsync(pilotRank);
            if (await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<PilotType> SavePilotType(PilotType pilotType)
        {
            var savedEntity = await _context.PilotTypes.AddAsync(pilotType);
            if (await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<PilotRank> UpdatePilotRank(PilotRank pilotRank)
        {
            var updatedEntity = _context.PilotRanks.Update(pilotRank);
            if (await SaveChanges())
            {
                return updatedEntity.Entity;
            }
            return null;
        }

        public async Task<PilotType> UpdatePilotType(PilotType pilotType)
        {
            var updatedEntity = _context.PilotTypes.Update(pilotType);
            if (await SaveChanges())
            {
                return updatedEntity.Entity;
            }
            return null;
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
