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
        Task<CommanderSMORoutesResourceTie> SaveCommanderTie(CommanderSMORoutesResourceTie commanderProfileTie);

        Task<CommanderSMORoutesResourceTie> FindCommanderTieById(long Id);

        //CommanderSMORoutesResourceTie FindCommanderUserProfileTieById(long Id);

        CommanderSMORoutesResourceTie GetResourceRegIdRegionAndRouteId(long regRessourceId, long RouteId, long RegionId);

        Task<IEnumerable<CommanderSMORoutesResourceTie>> FindAllCommanderTies();
        Task<bool> DeleteCommanderTie(CommanderSMORoutesResourceTie commanderProfileTie);
    }
}
