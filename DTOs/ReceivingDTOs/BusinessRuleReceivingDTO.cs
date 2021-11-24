using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class BusinessRuleReceivingDTO
    {
        [Required]
        public long? ServiceRegistrationId { get; set; }
        [Required]
        public bool IsQuantityRequired { get; set; }
        public long MaxQuantity { get; set; }
        [Required]
        public bool IsPairingRequired { get; set; }
    }
}
