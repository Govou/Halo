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
    public class CommanderRepositoryImpl:ICommanderRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<CommanderRepositoryImpl> _logger;
        public CommanderRepositoryImpl(HalobizContext context, ILogger<CommanderRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<CommanderType> SaveCommanderType(CommanderType commanderType)
        {
            var savedEntity = await _context.CommanderTypes.AddAsync(commanderType);
            if (await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<CommanderType> FindCommanderTypeById(long? Id)
        {
            return await _context.CommanderTypes
                .FirstOrDefaultAsync(ct => ct.Id == Id);
            //.FirstOrDefaultAsync(region => region.Id == Id && region.IsDeleted == false);
        }

        public async Task<CommanderType> FindCommanderTypeByName(string name)
        {
            return await _context.CommanderTypes
                .FirstOrDefaultAsync(ct => ct.TypeName == name);
            //.FirstOrDefaultAsync(region => region.Id == Id && region.IsDeleted == false);
        }

        public async Task<IEnumerable<CommanderType>> FindAllCommanderTypes()
        {

            return await _context.CommanderTypes.Where(ct => ct.IsDeleted == false)
                .ToListAsync();
        }

        public async Task<CommanderType> UpdateCommanderType(CommanderType commanderType)
        {
            var updatedEntity = _context.CommanderTypes.Update(commanderType);
            if (await SaveChanges())
            {
                return updatedEntity.Entity;
            }
            return null;
        }

        public async Task<bool> DeleteCommanderType(CommanderType commanderType)
        {
            commanderType.IsDeleted = true;
            _context.CommanderTypes.Update(commanderType);
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

        //Rank

        public async Task<CommanderRank> SaveCommanderRank(CommanderRank commanderRank)
        {
            var savedEntity = await _context.CommanderRanks.AddAsync(commanderRank);
            if (await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
            //throw new NotImplementedException();
        }

        public async Task<CommanderRank> FindCommanderRankById(long Id)
        {
            return await _context.CommanderRanks
               .FirstOrDefaultAsync(cr => cr.Id == Id && cr.IsDeleted == false);
            //.FirstOrDefaultAsync(region => region.Id == Id && region.IsDeleted == false);
        }

        public async Task<IEnumerable<CommanderRank>> FindAllCommanderRanks()
        {
            return await _context.CommanderRanks.Where(cr => cr.IsDeleted == false)
                .ToListAsync();
        }

        public long FindAllCommanderRanksCount(long? ctype)
        {
            var vv =  _context.CommanderRanks.Where(cr => cr.IsDeleted == false && cr.CommanderTypeId == ctype)
                .ToList();
            long count = vv.Count();
            return count;
        }

        public async Task<CommanderRank> UpdateCommanderRank(CommanderRank commanderRank)
        {
            var updatedEntity = _context.CommanderRanks.Update(commanderRank);
            if (await SaveChanges())
            {
                return updatedEntity.Entity;
            }
            return null;
        }

        public async Task<bool> DeleteCommanderRank(CommanderRank commanderRank)
        {
            commanderRank.IsDeleted = true;
            _context.CommanderRanks.Update(commanderRank);
            return await SaveChanges();
        }

        public CommanderRank GetRankname(string rankName)
        {
            return _context.CommanderRanks.Where(c => c.RankName == rankName && c.IsDeleted == false).FirstOrDefault();
            //return name.ToString();
        }

        public CommanderType GetTypename(string Name)
        {
            return _context.CommanderTypes.Where(c => c.TypeName == Name && c.IsDeleted == false).FirstOrDefault();
        }
    }
}
