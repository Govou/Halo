using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class ProjectAllocationDTO
    {
        public long ManagerId { get; set; }
        public string ManagerEmail { get; set; }
        public string ManagerImageUrl { get; set; }
        public string ManagerMobileNo { get; set; }
        public string ManagerName { get; set; }
        public bool IsMoved { get; set; }
       
        public string ServiceCategory { get; set; }
        public long? ServiceCategoryId { get; set; }
      
        public string ServiceGroup { get; set; }
        public long? ServiceGroupId { get; set; }
        public string MarketArea { get; set; }
       
        public Division Division { get; set; }
        public long? DivisionId { get; set; }
        [Key]
        public long Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
