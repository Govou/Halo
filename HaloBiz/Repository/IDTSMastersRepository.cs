using HaloBiz.DTOs.ReceivingDTOs;
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

        Task<ArmedEscortDTSMaster> FindArmedEscortMasterByResourceId(long resourceId);
        Task<ArmedEscortDTSMaster> FindArmedEscortMasterByResourceId2(long? resourceId);
        Task<ArmedEscortDTSMasterExtended> FindAllArmedEscortMastersForAutoAssignmentByPickupDate(long seviceRegId, long RouteId, DateTime pickupDate, DateTime pickUpTime);
        //Task<IEnumerable<ArmedEscortDTSMaster>> FindAllArmedEscortMastersForAutoAssignmentByPickupDate(DateTime pickupDate, DateTime pickUpTime);
        Task<IEnumerable<ArmedEscortDTSMaster>> FindAllArmedEscortMasters();

        ArmedEscortDTSMaster GetTypename(string Name);
        ArmedEscortDTSMaster GetArmedEscortProfileId(long? profileId);

        Task<ArmedEscortDTSMaster> UpdateArmedEscortMaster(ArmedEscortDTSMaster armedEscort);

        Task<bool> DeleteArmedEscortMaster(ArmedEscortDTSMaster armedEscort);

        //Commander
        Task<CommanderDTSMaster> SaveCommanderMaster(CommanderDTSMaster commander);

        Task<CommanderDTSMaster> FindCommanderMasterById(long Id);
        Task<CommanderDTSMaster> FindCommanderMasterByResourceId(long resourceId);
        Task<CommanderDTSMaster> FindCommanderMasterByResourceId2(long? resourceId);
        Task<CommanderDTSMasterExtended> FindAllCommanderMastersForAutoAssignmentByPickupDate(long seviceRegId, long RouteId, DateTime pickupDate, DateTime pickUpTime);
        //Task<IEnumerable<CommanderDTSMaster>> FindAllCommanderMastersForAutoAssignmentByPickupDate(long seviceRegId, long RouteId, DateTime pickupDate, DateTime pickUpTime);
        //Task<IEnumerable<CommanderDTSMaster>> FindAllCommanderMastersForAutoAssignmentByPickupDate(DateTime pickupDate, DateTime pickUpTime);
        Task<IEnumerable<CommanderDTSMaster>> FindAllCommanderMasters();

        CommanderDTSMaster GetCommandername(string Name);
        CommanderDTSMaster GetCommanderProfileId(long? profileId);

        Task<CommanderDTSMaster> UpdateCommanderMaster(CommanderDTSMaster commander);

        Task<bool> DeleteCommanderMaster(CommanderDTSMaster commander);

        //Pilot
        Task<PilotDTSMaster> SavePilotMaster(PilotDTSMaster pilot);

        Task<PilotDTSMaster> FindPilotMasterById(long Id);
        Task<PilotDTSMaster> FindPilotMasterByResourceId(long resourceId);
        Task<PilotDTSMaster> FindPilotMasterByResourceId2(long? resourceId);
        Task<PilotDTSMasterExtended> FindAllPilotMastersForAutoAssignmentByPickupDate(long seviceRegId, long RouteId, DateTime pickupDate, DateTime pickUpTime);
        //Task<IEnumerable<PilotDTSMaster>> FindAllPilotMastersForAutoAssignmentByPickupDate_(long seviceRegId, long RouteId, DateTime pickupDate, DateTime pickUpTime);
        //Task<IEnumerable<PilotDTSMaster>> FindAllPilotMastersForAutoAssignmentByPickupDate(DateTime pickupDate, DateTime pickUpTime);
        Task<IEnumerable<PilotDTSMaster>> FindAllPilotMasters();

        PilotDTSMaster GetPilotname(string Name);
        PilotDTSMaster GetPilotProfileId(long? profileId);

        Task<PilotDTSMaster> UpdatePilotMaster(PilotDTSMaster pilot);

        Task<bool> DeletePilotMaster(PilotDTSMaster pilot);

        //Vehicle
        Task<VehicleDTSMaster> SaveVehicleMaster(VehicleDTSMaster vehicle);

        Task<VehicleDTSMaster> FindVehicleMasterById(long Id);
        Task<VehicleDTSMaster> FindVehicleMasterByResourceId(long resourceId);
        Task<VehicleDTSMaster> FindVehicleMasterByResourceId2(long? resourceId);

        Task<IEnumerable<VehicleDTSMaster>> FindAllVehicleMasters();
        Task<IEnumerable<VehicleDTSMaster>> FindAllVehicleMastersForAutoAssignment();
        //Task<IEnumerable<VehicleDTSMaster>> FindAllVehicleMastersForAutoAssignmentByPickupDate(DateTime pickupDate, DateTime pickUpTime);
        Task<VehicleDTSMasterExtended> FindAllVehicleMastersForAutoAssignmentByPickupDate(long seviceRegId, long RouteId, DateTime pickupDate, DateTime pickUpTime);
        //Task<IEnumerable<VehicleDTSMaster>> FindAllVehicleMastersForAutoAssignmentByPickupDate_(long seviceRegId, long RouteId, DateTime pickupDate, DateTime pickUpTime);

        VehicleDTSMaster GetVehiclename(string Name);
        VehicleDTSMaster GetVehicleProfileId(long? profileId);
        Task<VehicleDTSMaster> UpdatevehicleMaster(VehicleDTSMaster vehicle);

        Task<bool> DeleteVehicleMaster(VehicleDTSMaster vehicle);
    }
}
