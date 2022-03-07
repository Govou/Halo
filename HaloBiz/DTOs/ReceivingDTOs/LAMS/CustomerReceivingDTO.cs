using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.ReceivingDTOs.LAMS
{
    public class CustomerReceivingDTO
    {
        [Required, MinLength(3), MaxLength(100)]
        public string GroupName { get; set; }
        [Required, RegularExpression("\\d{10,15}")]
        public string PhoneNumber { get; set; }
        [Required, RegularExpression("^[_A-Za-z0-9-\\+]+(\\.[_A-Za-z0-9-]+)*@[A-Za-z0-9-]+(\\.[A-Za-z0-9]+)*(\\.[A-Za-z]{2,})$", ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [Required]
        public string RCNumber { get; set; }
        public long GroupTypeId { get; set; }
        [StringLength(5000)]
        public string LogoUrl { get; set; }
    }
}
