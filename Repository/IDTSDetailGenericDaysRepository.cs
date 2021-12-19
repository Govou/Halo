using HalobizMigrations.Models.Armada;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository
{
    public interface IDTSDetailGenericDaysRepository
    {
        //Armed Escort
        Task<ArmedEscortDTSDetailGenericDay> SaveArmedEscortGeneric(ArmedEscortDTSDetailGenericDay armedEscort);

        Task<ArmedEscortDTSDetailGenericDay> FindArmedEscortGenericById(long Id);

        Task<IEnumerable<ArmedEscortDTSDetailGenericDay>> FindAllArmedEscortGenerics();

        ArmedEscortDTSDetailGenericDay GetTypename(string Name);

        Task<ArmedEscortDTSDetailGenericDay> UpdateArmedEscortGeneric(ArmedEscortDTSDetailGenericDay armedEscort);

        Task<bool> DeleteArmedEscortGeneric(ArmedEscortDTSDetailGenericDay armedEscort);

        //Commander
        Task<CommanderDTSDetailGenericDay> SaveCommanderGeneric(CommanderDTSDetailGenericDay commander);

        Task<CommanderDTSDetailGenericDay> FindCommanderGenericById(long Id);

        Task<IEnumerable<CommanderDTSDetailGenericDay>> FindAllCommanderGenerics();

        CommanderDTSDetailGenericDay GetCommandername(string Name);

        Task<CommanderDTSDetailGenericDay> UpdateCommanderGeneric(CommanderDTSDetailGenericDay commander);

        Task<bool> DeleteCommanderGeneric(CommanderDTSDetailGenericDay commander);

        //Pilot
        Task<PilotDTSDetailGenericDay> SavePilotGeneric(PilotDTSDetailGenericDay pilot);

        Task<PilotDTSDetailGenericDay> FindPilotGenericById(long Id);

        Task<IEnumerable<PilotDTSDetailGenericDay>> FindAllPilotGenerics();

        PilotDTSDetailGenericDay GetPilotname(string Name);

        Task<PilotDTSDetailGenericDay> UpdatePilotGeneric(PilotDTSDetailGenericDay pilot);

        Task<bool> DeletePilotGeneric(PilotDTSDetailGenericDay pilot);

        //Vehicle
        Task<VehicleDTSDetailGenericDay> SaveVehicleGeneric(VehicleDTSDetailGenericDay vehicle);

        Task<VehicleDTSDetailGenericDay> FindVehicleGenericById(long Id);

        Task<IEnumerable<VehicleDTSDetailGenericDay>> FindAllVehicleGenerics();

        VehicleDTSDetailGenericDay GetVehiclename(string Name);

        Task<VehicleDTSDetailGenericDay> UpdatevehicleGenric(VehicleDTSDetailGenericDay vehicle);

        Task<bool> DeleteVehicleGeneric(VehicleDTSDetailGenericDay vehicle);
    }
}
