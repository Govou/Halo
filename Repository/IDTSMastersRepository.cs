using HalobizMigrations.Models.Armada;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository
{
    public interface IDTSMastersRepository
    {
        //Armed Escort
        Task<ArmedEscortDTSMaster> SaveArmedEscortMaster(ArmedEscortDTSMaster armedEscort);

        Task<ArmedEscortDTSMaster> FindArmedEscortMasterById(long Id);

        Task<IEnumerable<ArmedEscortDTSMaster>> FindAllArmedEscortMasters();

        ArmedEscortDTSMaster GetTypename(string Name);
        ArmedEscortDTSMaster GetArmedEscortProfileId(long? profileId);

        Task<ArmedEscortDTSMaster> UpdateArmedEscortMaster(ArmedEscortDTSMaster armedEscort);

        Task<bool> DeleteArmedEscortMaster(ArmedEscortDTSMaster armedEscort);

        //Commander
        Task<CommanderDTSMaster> SaveCommanderMaster(CommanderDTSMaster commander);

        Task<CommanderDTSMaster> FindCommanderMasterById(long Id);

        Task<IEnumerable<CommanderDTSMaster>> FindAllCommanderMasters();

        CommanderDTSMaster GetCommandername(string Name);
        CommanderDTSMaster GetCommanderProfileId(long? profileId);

        Task<CommanderDTSMaster> UpdateCommanderMaster(CommanderDTSMaster commander);

        Task<bool> DeleteCommanderMaster(CommanderDTSMaster commander);

        //Pilot
        Task<PilotDTSMaster> SavePilotMaster(PilotDTSMaster pilot);

        Task<PilotDTSMaster> FindPilotMasterById(long Id);

        Task<IEnumerable<PilotDTSMaster>> FindAllPilotMasters();

        PilotDTSMaster GetPilotname(string Name);
        PilotDTSMaster GetPilotProfileId(long? profileId);

        Task<PilotDTSMaster> UpdatePilotMaster(PilotDTSMaster pilot);

        Task<bool> DeletePilotMaster(PilotDTSMaster pilot);

        //Vehicle
        Task<VehicleDTSMaster> SaveVehicleMaster(VehicleDTSMaster vehicle);

        Task<VehicleDTSMaster> FindVehicleMasterById(long Id);

        Task<IEnumerable<VehicleDTSMaster>> FindAllVehicleMasters();

        VehicleDTSMaster GetVehiclename(string Name);
        VehicleDTSMaster GetVehicleProfileId(long? profileId);
        Task<VehicleDTSMaster> UpdatevehicleMaster(VehicleDTSMaster vehicle);

        Task<bool> DeleteVehicleMaster(VehicleDTSMaster vehicle);
    }
}
