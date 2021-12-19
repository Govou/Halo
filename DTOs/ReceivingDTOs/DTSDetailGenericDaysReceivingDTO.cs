using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class DTSDetailGenericDaysReceivingDTO
    {
    }
    public class ArmedEscortDTSDetailGenericDaysReceivingDTO
    {
        public bool Sunday { get; set; }
        public bool Saturday { get; set; }
        public bool Friday { get; set; }
        public bool Thursday { get; set; }
        public bool Wednesday { get; set; }
        public bool Tuesday { get; set; }
        public bool Monday { get; set; }
        public DateTime ClosingTime { get; set; }
        public DateTime OpeningTime { get; set; }
        public long? DTSMasterId { get; set; }
    }
    public class CommanderDTSDetailGenericDaysReceivingDTO
    {
        public bool Sunday { get; set; }
        public bool Saturday { get; set; }
        public bool Friday { get; set; }
        public bool Thursday { get; set; }
        public bool Wednesday { get; set; }
        public bool Tuesday { get; set; }
        public bool Monday { get; set; }
        public DateTime ClosingTime { get; set; }
        public DateTime OpeningTime { get; set; }
        public long? DTSMasterId { get; set; }
    }
    public class PilotDTSDetailGenericDaysReceivingDTO
    {
        public bool Sunday { get; set; }
        public bool Saturday { get; set; }
        public bool Friday { get; set; }
        public bool Thursday { get; set; }
        public bool Wednesday { get; set; }
        public bool Tuesday { get; set; }
        public bool Monday { get; set; }
        public DateTime ClosingTime { get; set; }
        public DateTime OpeningTime { get; set; }
        public long? DTSMasterId { get; set; }
    }
    public class VehicleDTSDetailGenericDaysReceivingDTO
    {
        public bool Sunday { get; set; }
        public bool Saturday { get; set; }
        public bool Friday { get; set; }
        public bool Thursday { get; set; }
        public bool Wednesday { get; set; }
        public bool Tuesday { get; set; }
        public bool Monday { get; set; }
        public DateTime ClosingTime { get; set; }
        public DateTime OpeningTime { get; set; }
        public long? DTSMasterId { get; set; }
    }
}
