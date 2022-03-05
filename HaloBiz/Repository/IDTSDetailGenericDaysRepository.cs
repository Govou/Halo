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

        Task<IEnumerable<ArmedEscortDTSDetailGenericDay>> FindArmedEscortGenericByMasterId(long masterId);

        Task<IEnumerable<ArmedEscortDTSDetailGenericDay>> FindAllArmedEscortGenerics();

        ArmedEscortDTSDetailGenericDay GetTypename(string Name);

        ArmedEscortDTSDetailGenericDay GetArmedEscortDTSMasterId(long? dtsMasterId);

        Task<ArmedEscortDTSDetailGenericDay> UpdateArmedEscortGeneric(ArmedEscortDTSDetailGenericDay armedEscort);

        Task<bool> DeleteArmedEscortGeneric(ArmedEscortDTSDetailGenericDay armedEscort);

        //Commander
        Task<CommanderDTSDetailGenericDay> SaveCommanderGeneric(CommanderDTSDetailGenericDay commander);

        Task<CommanderDTSDetailGenericDay> FindCommanderGenericById(long Id);
        Task<IEnumerable<CommanderDTSDetailGenericDay>> FindCommanderGenericByMasterId(long masterId);

        Task<IEnumerable<CommanderDTSDetailGenericDay>> FindAllCommanderGenerics();

        CommanderDTSDetailGenericDay GetCommandername(string Name);
        CommanderDTSDetailGenericDay GetCommanderDTSMasterId(long? dtsMasterId);

        Task<CommanderDTSDetailGenericDay> UpdateCommanderGeneric(CommanderDTSDetailGenericDay commander);

        Task<bool> DeleteCommanderGeneric(CommanderDTSDetailGenericDay commander);

        //Pilot
        Task<PilotDTSDetailGenericDay> SavePilotGeneric(PilotDTSDetailGenericDay pilot);

        Task<PilotDTSDetailGenericDay> FindPilotGenericById(long Id);
        Task<IEnumerable<PilotDTSDetailGenericDay>> FindPilotGenericByMasterId(long masterId);
        Task<IEnumerable<PilotDTSDetailGenericDay>> FindAllPilotGenerics();

        PilotDTSDetailGenericDay GetPilotname(string Name);
        PilotDTSDetailGenericDay GetPilotDTSMasterId(long? dtsMasterId);

        Task<PilotDTSDetailGenericDay> UpdatePilotGeneric(PilotDTSDetailGenericDay pilot);

        Task<bool> DeletePilotGeneric(PilotDTSDetailGenericDay pilot);

        //Vehicle
        Task<VehicleDTSDetailGenericDay> SaveVehicleGeneric(VehicleDTSDetailGenericDay vehicle);

        Task<VehicleDTSDetailGenericDay> FindVehicleGenericById(long Id);
        Task<IEnumerable<VehicleDTSDetailGenericDay>> FindVehicleGenericByMasterId(long masterId);

        Task<IEnumerable<VehicleDTSDetailGenericDay>> FindAllVehicleGenerics();

        VehicleDTSDetailGenericDay GetVehiclename(string Name);
        VehicleDTSDetailGenericDay GetVehicleDTSMasterId(long? dtsMasterId);

        Task<VehicleDTSDetailGenericDay> UpdatevehicleGenric(VehicleDTSDetailGenericDay vehicle);

        Task<bool> DeleteVehicleGeneric(VehicleDTSDetailGenericDay vehicle);
    }
}
