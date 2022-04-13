using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.ReceivingDTOs.LAMS
{
    public class ContractApprovalDTO
    {
        public bool isApproved { get; set; }
        public long approvalId { get; set; }
        public long contractId { get; set; }
    }
}
