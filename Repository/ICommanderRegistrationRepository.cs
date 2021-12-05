using HalobizMigrations.Models.Armada;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository
{
    public interface ICommanderRegistrationRepository
    {
        Task<CommanderProfile> SaveCommander(CommanderProfile commanderProfile);

        Task<CommanderProfile> FindCommanderById(long Id);

        CommanderProfile FindCommanderUserProfileById(long Id);

        //Task<CommanderProfile> FindCommanderTypeByName(string name);
        //CommanderProfile GetTypename(string Name);

        Task<IEnumerable<CommanderProfile>> FindAllCommanders();
        //long FindAllCommanderRanksCount(string ctype);
        Task<CommanderProfile> UpdateCommander(CommanderProfile commanderProfile);
        Task<bool> DeleteCommander(CommanderProfile commanderProfile);

        //Tie
        //Task<CommanderProfile> SaveCommanderTie(CommanderProfile commanderProfile);

        //Task<CommanderProfile> FindCommanderTieById(long Id);

        //CommanderProfile FindCommanderUserProfileTieById(long Id);

        //CommanderProfile GetResourceRegIdRegionAndRouteId(long regRessourceId, long RouteId, long RegionId);

        //Task<IEnumerable<CommanderProfile>> FindAllCommanderTies();
        //Task<bool> DeleteCommanderTie(CommanderProfile commanderProfile);
    }
}
