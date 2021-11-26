using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class BRPairableReceivingDTO
    {
       [Required]
        public long? BusinessRuleId { get; set; }
        [Required]
        public long? ServiceRegistrationId { get; set; }
    }
}
