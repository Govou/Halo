using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class PickComplaintDTO
    {
        [Required]
        public long complaintId { get; set; }
    }
}
