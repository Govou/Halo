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
        Task<VehicleType> UpdateVehicleTypes(VehicleType vehicleType);
        Task<CommanderType> UpdateCommanderTypess(CommanderType commanderType);
        Task<ArmedEscortType> UpdateArmedEscortTypes(ArmedEscortType armedEscortType);
        Task<PilotType> UpdatePilotTypes(PilotType pilotType);
    }
}
