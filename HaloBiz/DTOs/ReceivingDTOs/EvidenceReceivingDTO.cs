using HalobizMigrations.Models.Complaints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class EvidenceReceivingDTO
    {
        public string Caption { get; set; }
        public string ImageUrl { get; set; }
        public long? ComplaintId { get; set; }
        public ComplaintStage ComplaintStage { get; set; }
    }
}
