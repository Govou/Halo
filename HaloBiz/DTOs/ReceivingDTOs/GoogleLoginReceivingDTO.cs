using System.ComponentModel.DataAnnotations;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class GoogleLoginReceivingDTO
    {
        [Required]
        public string IdToken { get; set; }
    }
}
