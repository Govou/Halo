using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class MoveComplaintToNextStageDTO
    {
        [Required]
        public long complaintID { get; set; }
        public string findings { get; set; }
        public string details { get; set; }
        [Required]
        [Range(1, 5)]
        public int currentStage { get; set; }       //Uses ComplaintStage
        public string[] evidences { get; set; }
        public string applicationUrl { get; set; }
    }
}
