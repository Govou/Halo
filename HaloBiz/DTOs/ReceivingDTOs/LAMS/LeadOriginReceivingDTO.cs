using System.ComponentModel.DataAnnotations;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class LeadOriginReceivingDTO
    {
        [Required]
        public string Caption { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        [Required]
        public long LeadTypeId { get; set; }
    }
}