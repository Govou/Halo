using System.ComponentModel.DataAnnotations;

namespace Halobiz.Common.DTOs.ReceivingDTOs
{
    public class GoogleLoginReceivingDTO
    {
        [Required]
        public string IdToken { get; set; }
    }
}
