using System.ComponentModel.DataAnnotations;

namespace OnlinePortalBackend.DTOs.ReceivingDTOs
{
    public class SecurityQuestionReceivingDTO
    {
        public long ClientRegistrationId { get; set; }
        [Required]
        public string Question { get; set; }
    }
}
