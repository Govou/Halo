using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class DivisionReceivingDTO
    {
        [Required, MinLength(3), MaxLength(100)]
        public string Name { get; set; }
        [Required, MinLength(3), MaxLength(255)]
        public string Description { get; set; }
        public string MissionStatement { get; set; }
        [Required]
        public long HeadId { get; set; }
    }
}
