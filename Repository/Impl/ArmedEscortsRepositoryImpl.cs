using HalobizMigrations.Data;
using HalobizMigrations.Models;
using HalobizMigrations.Models.Armada;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository.Impl
{
    public class ArmedEscortsRepositoryImpl : IArmedEscortsRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<ArmedEscortsRepositoryImpl> _logger;
        public ArmedEscortsRepositoryImpl(HalobizContext context, ILogger<ArmedEscortsRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }
        public async Task<bool> DeleteArmedEscortRank(ArmedEscortRank armedEscortRank)
        {
            armedEscortRank.IsDeleted = true;
            _context.ArmedEscortRanks.Update(armedEscortRank);
            return await SaveChanges();
        }

        public async Task<bool> DeleteArmedEscortType(ArmedEscortType armedEscortType)
        {
            armedEscortType.IsDeleted = true;
            _context.ArmedEscortTypes.Update(armedEscortType);
            return await SaveChanges();
        }

        public async Task<IEnumerable<ArmedEscortRank>> FindAllArmedEscortRanks()
        {
            return await _context.ArmedEscortRanks.Where(rank=>rank.IsDeleted == false)
                .ToListAsync();
        }

        public async Task<IEnumerable<ArmedEscortType>> FindAllArmedEscortTypes()
        {
            return await _context.ArmedEscortTypes.Where(rank => rank.IsDeleted == false)
               .ToListAsync();
        }

        public async Task<ArmedEscortRank> FindArmedEscortRankById(long Id)
        {
            return await _context.ArmedEscortRanks.FirstOrDefaultAsync(aer => aer.Id == Id && aer.IsDeleted == false);
            //.FirstOrDefaultAsync(aer => aer.Id == Id);

        }

        public long FindAllArmedEscortRanksCount(long ctypeId)
        {
            var vv = _context.ArmedEscortRanks.Where(aer => aer.IsDeleted == false && aer.ArmedEscortTypeId == ctypeId)
                .ToList();
            long count = vv.Count();
            return count;
        }

        public ArmedEscortRank GetRankname(string rankName)
        {
            return _context.ArmedEscortRanks.Where(c=>c.RankName == rankName && c.IsDeleted == false).FirstOrDefault();
            //return name.ToString();
        }

        public ArmedEscortType GetTypename(string Name)
        {
            return _context.ArmedEscortTypes.Where(c => c.Name == Name && c.IsDeleted == false).FirstOrDefault();
            //return name.ToString();
        }
        public async Task<ArmedEscortType> FindArmedEscortTypeById(long Id)
        {
            return await _context.ArmedEscortTypes
                                      .FirstOrDefaultAsync(aet => aet.Id == Id && aet.IsDeleted == false);
        }

        public async Task<ArmedEscortRank> SaveArmedEscortRank(ArmedEscortRank armedEscortRank)
        {
            var savedEntity = await _context.ArmedEscortRanks.AddAsync(armedEscortRank);
            if (await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<ArmedEscortType> SaveArmedEscortType(ArmedEscortType armedEscortType)
        {
            var savedEntity = await _context.ArmedEscortTypes.AddAsync(armedEscortType);
            if (await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<ArmedEscortRank> UpdateArmedEscortRank(ArmedEscortRank armedEscortRank)
        {
            var updatedEntity = _context.ArmedEscortRanks.Update(armedEscortRank);
            if (await SaveChanges())
            {
                return updatedEntity.Entity;
            }
            return null;
        }

        public async Task<ArmedEscortType> UpdateArmedEscortType(ArmedEscortType armedEscortType)
        {
            var updatedEntity = _context.ArmedEscortTypes.Update(armedEscortType);
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
