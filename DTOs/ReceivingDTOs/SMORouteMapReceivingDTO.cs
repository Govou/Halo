using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class SMORouteMapReceivingDTO
    {
        public long?[] StateId { get; set; }

        public long? SMORegionId { get; set; }

        public long? SMORouteId { get; set; }

        public string Comment { get; set; }
    }
}
