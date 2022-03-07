using System.ComponentModel.DataAnnotations;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class LeadTypeReceivingDTO
    {
        [Required]
        public string Caption { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
    }
}