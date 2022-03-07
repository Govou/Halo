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
        // Task<CommanderSMORoutesResourceTie> FindCommanderTieByResourceId(long resourceId);
        Task<IEnumerable<CommanderSMORoutesResourceTie>> FindAllCommanderTiesByResourceId(long resourceId);


        CommanderSMORoutesResourceTie GetResourceRegIdRegionAndRouteId(long regRessourceId, long? RouteId);
        CommanderSMORoutesResourceTie GetResourceRegIdRegionAndRouteId2(long? regRessourceId, long? RouteId);
        public IEnumerable<CommanderSMORoutesResourceTie> GetAllCommanderssOnRouteByResourceAndRouteId(long? RouteId);
        Task<IEnumerable<CommanderSMORoutesResourceTie>> FindAllCommanderTies();
        Task<bool> DeleteCommanderTie(CommanderSMORoutesResourceTie commanderProfileTie);
    }
}
