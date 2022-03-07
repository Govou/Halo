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
        Task<VehicleResourceRequiredPerService> FindVehicleResourceByServiceRegId(long seviceRegId);
        Task<IEnumerable<VehicleResourceRequiredPerService>> FindAllVehicleResourceByServiceRegId(long seviceRegId);
        VehicleResourceRequiredPerService GetVehicleResourceApplicableTypeReqById(long serviceRegId, long? applicableTypeId);
        Task<IEnumerable<VehicleResourceRequiredPerService>> FindAllVehicleResources();
        Task<bool> DeleteVehicleResource(VehicleResourceRequiredPerService vehicle);
       
        VehicleResourceRequiredPerService GetVehicleTypeAndRegServiceId(long RegServiceId,long? VehicleTypeId);

        //Pilot
        Task<PilotResourceRequiredPerService> SavePilotResource(PilotResourceRequiredPerService pilot);
        Task<PilotResourceRequiredPerService> FindPilotResourceById(long Id);
        Task<PilotResourceRequiredPerService> FindPilotResourceByServiceRegId(long seviceRegId);
        PilotResourceRequiredPerService GetPilotResourceApplicableTypeReqById(long serviceRegId, long? applicableTypeId);
        Task<IEnumerable<PilotResourceRequiredPerService>> FindAllPilotResources();
        Task<bool> DeletePilotResource(PilotResourceRequiredPerService pilot);
        PilotResourceRequiredPerService GetPilotTypeAndRegServiceId(long RegServiceId, long? PilotTypeId);

        //Commander
        Task<CommanderResourceRequiredPerService> SaveCommanderResource(CommanderResourceRequiredPerService commander);
        Task<CommanderResourceRequiredPerService> FindCommanderResourceById(long Id);
        Task<CommanderResourceRequiredPerService> FindCommanderResourceByServiceRegId(long seviceRegId);
        CommanderResourceRequiredPerService GetCommanderResourceApplicableTypeReqById(long serviceRegId, long? applicableTypeId);
        Task<IEnumerable<CommanderResourceRequiredPerService>> FindAllCommanderResources();
        Task<bool> DeleteCommanderResource(CommanderResourceRequiredPerService commander);
        CommanderResourceRequiredPerService GetCommanderTypeAndRegServiceId(long RegServiceId, long? CommanderTypeId);

        //AEscort
        Task<ArmedEscortResourceRequiredPerService> SaveArmedEscortResource(ArmedEscortResourceRequiredPerService escort);
        Task<ArmedEscortResourceRequiredPerService> FindArmedEscortResourceById(long Id);
        Task<ArmedEscortResourceRequiredPerService> FindArmedEscortResourceByServiceRegId(long seviceRegId);
        ArmedEscortResourceRequiredPerService GetArmedEscortResourceApplicableTypeReqById(long serviceRegId, long? applicableTypeId);
        Task<IEnumerable<ArmedEscortResourceRequiredPerService>> FindAllArmedEscortResources();
        Task<bool> DeleteArmedEscortResource(ArmedEscortResourceRequiredPerService escort);
        ArmedEscortResourceRequiredPerService GetArmedEscortTypeAndRegServiceId(long RegServiceId, long? ArmedEscortTypeId);

    }
}
