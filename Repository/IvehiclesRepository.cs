using HalobizMigrations.Models.Armada;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository
{
    public interface IVehiclesRepository
    {
        
        Task<Vehicle> SaveVehicle(Vehicle vehicle);

        Task<Vehicle> FindVehicleById(long Id);

        Task<IEnumerable<Vehicle>> FindAllVehicles();

        Task<Vehicle> UpdateVehicle(Vehicle vehicle);

        Task<bool> DeleteVehicle(Vehicle vehicle);

        //Rank
        Task<VehicleType> SaveVehicleType(VehicleType vehicleType);

        Task<VehicleType> FindVehicleTypeById(long Id);
        VehicleType GetTypename(string Name);

        Task<IEnumerable<VehicleType>> FindAllVehicleTypes();

        Task<VehicleType> UpdateVehicleType(VehicleType vehicleType);

        Task<bool> DeleteVehicleType(VehicleType vehicleType);
    }
}
