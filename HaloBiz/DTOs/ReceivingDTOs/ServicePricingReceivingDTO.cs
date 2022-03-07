using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class ServicePricingReceivingDTO
    {
        public long ServiceId { get; set; }
        public decimal MinimumAmount { get; set; }
        public decimal MaximumAmount { get; set; }
        public long BranchId { get; set; }
    }
}
