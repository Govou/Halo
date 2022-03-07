using System.ComponentModel.DataAnnotations;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class EndorsementTypeReceivingDTO
    {
        [Required]
        public string Caption { get; set; }
        [StringLength(255)]
        public string Description { get; set; }
    }
}