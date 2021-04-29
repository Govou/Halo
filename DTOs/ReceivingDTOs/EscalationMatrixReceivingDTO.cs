using HalobizMigrations.Models.Complaints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class EscalationMatrixReceivingDTO
    {
        public long ComplaintTypeId { get; set; }
        public long Level1MaxResolutionTimeInHrs { get; set; }
        public long Level2MaxResolutionTimeInHrs { get; set; }
        public long Level3MaxResolutionTimeInHrs { get; set; }
        public ICollection<EscalationMatrixUserProfile> ComplaintAttendants { get; set; }
    }
}
