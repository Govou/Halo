using System.ComponentModel.DataAnnotations;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class MakeReceivingDTO
    {
        [Required]
        [StringLength(50)]
        public string Caption { get; set; }
        public string Description { get; set; }
    }
}
