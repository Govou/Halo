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

        //Type
        //Task<PilotRank> SaveArmedEscortRank(PilotRank pilotRank);

        //Task<PilotRank> FindPilotRankById(long Id);

        //Task<IEnumerable<PilotRank>> FindAllPilotRanks();

        //Task<PilotRank> UpdatePilotRank(PilotRank ailotRank);

        //Task<bool> DeletePilotRank(PilotRank pilotRank);
    }
}
