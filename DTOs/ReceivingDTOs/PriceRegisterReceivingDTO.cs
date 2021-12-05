using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class PriceRegisterReceivingDTO
    {

        public decimal SellingPrice { get; set; }
        public decimal MarkupPrice { get; set; }
        public double MarkupPercentage { get; set; }

        public decimal CostPrice { get; set; }
        public long ServiceRegistrationId { get; set; }


        public long SMORouteId { get; set; }


        public long SMORegionId { get; set; }

    }

      
}
