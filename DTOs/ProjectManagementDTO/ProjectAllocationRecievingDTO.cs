using HalobizMigrations.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class ProjectAllocationRecievingDTO
    {
        public bool IsDeleted { get; set; }

        public long ManagerId { get; set; }
        public string ManagerEmail { get; set; }
        public string ManagerImageUrl { get; set; }
        public string ManagerMobileNo { get; set; }
        public string ManagerName { get; set; }
        public bool IsMoved { get; set; }
        public long? ServiceCategoryId { get; set; }
        public long? ServiceGroupId { get; set; }
        public long? OperatingEntityId { get; set; }
        public long? DivisionId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}

