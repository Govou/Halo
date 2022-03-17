using System.ComponentModel.DataAnnotations;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class ModelReceivingDTO
    {
        [Required]
        [StringLength(50)]
        public string Caption { get; set; }
        public string Description { get; set; }
        [Required]
        public long MakeId { get; set; }
    }
}