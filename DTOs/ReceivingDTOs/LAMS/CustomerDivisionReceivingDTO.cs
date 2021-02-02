using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.ReceivingDTOs.LAMS
{
    public class CustomerDivisionReceivingDTO
    {
        [StringLength(100)]
        public string Industry { get; set; }
        [StringLength(50)]
        public string RCNumber { get; set; }
        [StringLength(250)]
        public string DivisionName { get; set; }
        [Required, RegularExpression("\\d{10,15}")]
        public string PhoneNumber { get; set; }
        [Required, RegularExpression("^[_A-Za-z0-9-\\+]+(\\.[_A-Za-z0-9-]+)*@[A-Za-z0-9-]+(\\.[A-Za-z0-9]+)*(\\.[A-Za-z]{2,})$", ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [StringLength(5000)]
        public string LogoUrl { get; set; }
        [StringLength(1000)]
        public string Address { get; set; }
        public long CustomerId { get; set; }
    }
}
