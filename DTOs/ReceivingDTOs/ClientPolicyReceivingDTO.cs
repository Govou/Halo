using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class ClientPolicyReceivingDTO
    {
        public long CustomerDivisionId { get; set; }
        public long? ContractId { get; set; }
        public long? ContractServiceId { get; set; }
        public bool AutoRenew { get; set; }
        public long? RateReviewInterval { get; set; }
        public DateTime? NextRateReviewDate { get; set; }
    }
}
