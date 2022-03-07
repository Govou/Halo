using System.ComponentModel.DataAnnotations;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class BaseSetupDTO
    {
        [Required]
        public string Caption { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
    }
}