using System.ComponentModel.DataAnnotations;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class DropReasonReceivingDTO
    {
        [Required, MinLength(3), MaxLength(100)]
        public string Title { get; set; }
    }
}