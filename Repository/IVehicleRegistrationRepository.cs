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

        Task<Vehicle> UpdateVehicle(Vehicle vehicle);

        Task<bool> DeleteVehicle(Vehicle vehicle);
    }
}
