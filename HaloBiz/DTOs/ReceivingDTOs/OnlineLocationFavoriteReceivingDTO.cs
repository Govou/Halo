using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class OnlineLocationFavoriteReceivingDTO
    {
      
        public string LocationGeometry { get; set; }
        public double LocationLatitude { get; set; }
        public double LocationLongitude { get; set; }
        public string LocationFullAddress { get; set; }
        public string LocationStreetAddress { get; set; }
        public long LocationLGAId { get; set; }
        public long LocationStateId { get; set; }
        public long? OnlineProfileId { get; set; }
        public long ClientId { get; set; }

    
    }
}
