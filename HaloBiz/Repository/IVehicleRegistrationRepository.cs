using HalobizMigrations.Models.Armada;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository
{
    public interface IVehicleRegistrationRepository
    {
        Task<Vehicle> SaveVehicle(Vehicle vehicle);

        Task<Vehicle> FindVehicleById(long Id);

        Vehicle FindVehicleServiceById(long Id);

        Task<IEnumerable<Vehicle>> FindAllVehicles();
       // Task<IEnumerable<Vehicle>> FindAllVehicles2();

        Task<Vehicle> UpdateVehicle(Vehicle vehicle);

        Task<bool> DeleteVehicle(Vehicle vehicle);

        //Tie
        Task<VehicleSMORoutesResourceTie> SaveVehicleTie(VehicleSMORoutesResourceTie vehicleTie);

        Task<VehicleSMORoutesResourceTie> FindVehicleTieById(long Id);
        //Task<VehicleSMORoutesResourceTie> FindVehicleTieByResourceId(long resourceId);

        Task<IEnumerable<VehicleSMORoutesResourceTie>> FindVehicleTieByResourceId(long resourceId);
        Task<IEnumerable<VehicleSMORoutesResourceTie>> FindAllVehicleTies();

        VehicleSMORoutesResourceTie GetResourceRegIdRegionAndRouteId(long regRessourceId, long? RouteId);
        VehicleSMORoutesResourceTie GetResourceRegIdRegionAndRouteId2(long? regRessourceId, long? RouteId);
        IEnumerable<VehicleSMORoutesResourceTie> GetAllVehiclesOnRouteByResourceAndRouteId( long? RouteId);
       // IEnumerable<VehicleSMORoutesResourceTie> GetAllVehiclesOnRouteByResourceAndRouteId(long? regRessourceId, long? RouteId);
        Task<bool> DeleteVehicleTie(VehicleSMORoutesResourceTie vehicleTie);
    }
}
