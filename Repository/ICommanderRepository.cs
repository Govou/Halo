using HalobizMigrations.Models;
using HalobizMigrations.Models.Armada;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository
{
    public interface ICommanderRepository
    {
        //Type
        Task<CommanderType> SaveCommanderType(CommanderType commanderType);

        Task<CommanderType> FindCommanderTypeById(long Id);

        Task<CommanderType> FindCommanderTypeByName(string name);

        Task<IEnumerable<CommanderType>> FindAllCommanderTypes();
        long FindAllCommanderRanksCount(string ctype);
        Task<CommanderType> UpdateCommanderType(CommanderType commanderType); 

         Task<bool> DeleteCommanderType(CommanderType commanderType);

        //rank
        Task<CommanderRank> SaveCommanderRank(CommanderRank commanderRank);

        Task<CommanderRank> FindCommanderRankById(long Id);

        Task<IEnumerable<CommanderRank>> FindAllCommanderRanks();

        Task<CommanderRank> UpdateCommanderRank(CommanderRank commanderRank);

        Task<bool> DeleteCommanderRank(CommanderRank commanderRank);

    }
}
