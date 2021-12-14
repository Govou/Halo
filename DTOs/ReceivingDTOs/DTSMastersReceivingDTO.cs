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

}
