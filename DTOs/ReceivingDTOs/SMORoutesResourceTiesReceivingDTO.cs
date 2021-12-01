using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class SMORoutesResourceTiesReceivingDTO
    {
    }

    public class ArmedEscortSMORoutesResourceTieReceivingDTO
    {
        public long? ResourceId { get; set; }
        
        public long? SMORegionId { get; set; }
        public long? SMORouteId { get; set; }
     
    }
    public class PilotSMORoutesResourceTieReceivingDTO
    {
        public long? ResourceId { get; set; }
        public long? SMORegionId { get; set; }
        public long? SMORouteId { get; set; }
    }
    public class VehicleSMORoutesResourceTieReceivingDTO
    {
        public long? ResourceId { get; set; }
        public long? SMORegionId { get; set; }
        public long? SMORouteId { get; set; }
    }
    public class CommanderSMORoutesResourceTieReceivingDTO
    {
    }
}
