using System;
using System.ComponentModel.DataAnnotations;

namespace HaloBiz.DTOs.ReceivingDTOs.LAMS
{
    public class LeadReceivingDTO
    {
        [Required]
        public long LeadTypeId { get; set; }
        [Required]
        public long LeadOriginId { get; set; }
        [StringLength(100)]
        public string Industry { get; set; }
        [StringLength(50)]
        public string RCNumber { get; set; }
        [StringLength(250)]
        public string GroupName { get; set; }
        [Required]
        public long GroupTypeId { get; set; }
        [StringLength(5000)]
        public string LogoUrl { get; set; }
    }
}