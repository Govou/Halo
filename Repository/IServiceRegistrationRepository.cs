using HalobizMigrations.Models.Armada;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository
{
    public interface IServiceRegistrationRepository
    {
        Task<ServiceRegistration> SaveService(ServiceRegistration serviceRegistration);

        Task<ServiceRegistration> FindServiceById(long Id);

        Task<IEnumerable<ServiceRegistration>> FindAllServicess();

        ServiceRegistration GetserviceId(long serviceId);

        Task<ServiceRegistration> UpdateServices(ServiceRegistration serviceRegistration);

        Task<bool> DeleteService(ServiceRegistration serviceRegistration);

        //Applicable Types
        //Vehicle
        Task<VehicleResourceRequiredPerService> SaveResourceVehicle(VehicleResourceRequiredPerService vehicle);
        Task<VehicleResourceRequiredPerService> FindVehicleResourceById(long Id);
        Task<IEnumerable<VehicleResourceRequiredPerService>> FindAllVehicleResources();
        Task<bool> DeleteVehicleResource(VehicleResourceRequiredPerService vehicle);

        //Pilot
        Task<PilotResourceRequiredPerService> SavePilotResource(PilotResourceRequiredPerService pilot);
        Task<PilotResourceRequiredPerService> FindPilotResourceById(long Id);
        Task<IEnumerable<PilotResourceRequiredPerService>> FindAllPilotResources();
        Task<bool> DeletePilotResource(PilotResourceRequiredPerService pilot);

        //Commander
        Task<CommanderResourceRequiredPerService> SaveCommanderResource(CommanderResourceRequiredPerService commander);
        Task<CommanderResourceRequiredPerService> FindCommanderResourceById(long Id);
        Task<IEnumerable<CommanderResourceRequiredPerService>> FindAllCommanderResources();
        Task<bool> DeleteCommanderResource(CommanderResourceRequiredPerService commander);

        //AEscort
        Task<ArmedEscortResourceRequiredPerService> SaveArmedEscortResource(ArmedEscortResourceRequiredPerService escort);
        Task<ArmedEscortResourceRequiredPerService> FindArmedEsxortResourceById(long Id);
        Task<IEnumerable<ArmedEscortResourceRequiredPerService>> FindAllArmedEscortResources();
        Task<bool> DeleteArmedEscortResource(ArmedEscortResourceRequiredPerService escort);

    }
}
