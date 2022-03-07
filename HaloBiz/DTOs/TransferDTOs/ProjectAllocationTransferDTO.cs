using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class ProjectAllocationTransferDTO
    {
        public string Division { get; set; }
        public string MarketArea { get; set; }
        public string ServiceGroup { get; set; }
        public string ServiceCategory { get; set; }
        public bool IsMoved { get; set; }
        public string ManagerName { get; set; }
        public string ManagerMobileNo { get; set; }
        public string ManagerImageUrl { get; set; }
        public string ManagerEmail { get; set; }
        public long ManagerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
