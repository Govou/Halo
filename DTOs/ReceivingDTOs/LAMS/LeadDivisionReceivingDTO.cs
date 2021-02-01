using System.ComponentModel.DataAnnotations;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class LeadDivisionReceivingDTO
    {
        public long LeadOriginId { get; set; }
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
        [StringLength(1000)]
        public string LogoUrl { get; set; }
        [StringLength(1000)]
        public string Address { get; set; }
        public long? PrimaryContactId { get; set; }
        public long? SecondaryContactId { get; set; }
        public long? BranchId { get; set; }
        public long? OfficeId { get; set; }
        public long LeadId { get; set; }
    }
}