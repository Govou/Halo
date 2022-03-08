using HalobizMigrations.Models;
using HalobizMigrations.Models.Armada;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository
{
    public interface IArmedEscortRegistrationRepository
    {
        Task<ArmedEscortProfile> SaveArmedEscort(ArmedEscortProfile armedEscortProfile);

        Task<ArmedEscortProfile> FindArmedEscortById(long Id);

        Task<IEnumerable<ArmedEscortProfile>> FindAllArmedEscorts();

       // ArmedEscortProfile GetProfileName(string Name);

        Task<ArmedEscortProfile> UpdateArmedEscort(ArmedEscortProfile armedEscortProfile);

        Task<bool> DeleteArmedEscort(ArmedEscortProfile armedEscortProfile);

        //Tie
        Task<ArmedEscortSMORoutesResourceTie> SaveArmedEscortTie(ArmedEscortSMORoutesResourceTie armedEscortTie);

        Task<ArmedEscortSMORoutesResourceTie> FindArmedEscortTieById(long Id);
        Task<IEnumerable<ArmedEscortSMORoutesResourceTie>> FindArmedEscortTieByResourceId(long resourceId);  

        Task<IEnumerable<ArmedEscortSMORoutesResourceTie>> FindAllArmedEscortTies();

         ArmedEscortSMORoutesResourceTie GetServiceRegIdRegionAndRoute(long regRessourceId, long? RouteId);
         ArmedEscortSMORoutesResourceTie GetServiceRegIdRegionAndRoute2(long? regRessourceId, long? RouteId);
        IEnumerable<ArmedEscortSMORoutesResourceTie> GetAllArmedEscortsOnRouteByResourceAndRouteId(long? RouteId);

        //Task<ArmedEscortProfile> UpdateArmedEscort(ArmedEscortProfile armedEscortProfile);

        Task<bool> DeleteArmedEscortTie(ArmedEscortSMORoutesResourceTie armedEscortTie);
    }
}
