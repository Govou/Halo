using HalobizMigrations.Models.Armada;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class DTSMastersReceivingDTO
    {
    }
    public class ArmedEscortDTSMastersReceivingDTO
    {
        public string Caption  { get; set; }
        public long? ArmedEscortResourceId { get; set; }
        public DateTime AvailabilityStart { get; set; }
        public DateTime AvailablilityEnd { get; set; }
      
    }

    public class CommanderDTSMastersReceivingDTO
    {
        public string Caption { get; set; }
        public long CommanderResourceId { get; set; }
        public DateTime AvailabilityStart { get; set; }
        public DateTime AvailablilityEnd { get; set; }
    }

    public class PilotDTSMastersReceivingDTO
    {
        public string Caption { get; set; }
        public long PilotResourceId { get; set; }
        public DateTime AvailabilityStart { get; set; }
        public DateTime AvailablilityEnd { get; set; }
    }

    public class VehicleDTSMastersReceivingDTO
    {
        public string Caption { get; set; }
        public long VehicleResourceId { get; set; }
        public DateTime AvailabilityStart { get; set; }
        public DateTime AvailablilityEnd { get; set; }
    }

    //Vehicle
    public class VehicleDTSMasterExtended 
    {
        public IEnumerable<VehicleDTSMaster> eligibleVehiclesWithoutAssignment { get; set; }
        public IEnumerable<VehicleDTSMaster> eligibleVehiclesWithAssignment { get; set; }
    }

    //Pilot
    public class PilotDTSMasterExtended
    {
        public IEnumerable<PilotDTSMaster> eligiblePilotsWithoutAssignment { get; set; }
        public IEnumerable<PilotDTSMaster> eligiblePilotsWithAssignment { get; set; }
    }

    //Commander
    public class CommanderDTSMasterExtended
    {
        public IEnumerable<CommanderDTSMaster> eligibleCommandersWithoutAssignment { get; set; }
        public IEnumerable<CommanderDTSMaster> eligibleCommandersWithAssignment { get; set; }
    }

    //ArmedEscort
    public class ArmedEscortDTSMasterExtended
    {
        public IEnumerable<ArmedEscortDTSMaster> eligibleArmedEscortsWithoutAssignment { get; set; }
        public IEnumerable<ArmedEscortDTSMaster> eligibleArmedEscortsWithAssignment { get; set; }
    }
}
