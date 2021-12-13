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
        public long ResourceId { get; set; }
        public DateTime AvailabilityStart { get; set; }
        public DateTime AvailablilityEnd { get; set; }
    }

    public class CommanderDTSMastersReceivingDTO
    {
        public string Caption { get; set; }
        public long ResourceId { get; set; }
        public DateTime AvailabilityStart { get; set; }
        public DateTime AvailablilityEnd { get; set; }
    }

    public class PilotDTSMastersReceivingDTO
    {
        public string Caption { get; set; }
        public long ResourceId { get; set; }
        public DateTime AvailabilityStart { get; set; }
        public DateTime AvailablilityEnd { get; set; }
    }

    public class VehicleDTSMastersReceivingDTO
    {
        public string Caption { get; set; }
        public long ResourceId { get; set; }
        public DateTime AvailabilityStart { get; set; }
        public DateTime AvailablilityEnd { get; set; }
    }

}
