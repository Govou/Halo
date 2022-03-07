using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class StrategicBusinessUnitReceivingDTO
    {
        [Required, MinLength(3)]
        public string Name { get; set; }
        [Required, MinLength(3)]
        public string Description { get; set; }
        public string Alias { get; set; } //Cost centers
        [Required]
        public long OperatingEntityId { get; set; }
       
    }
}

