
using Halobiz.Common.DTOs.ReceivingDTO;

namespace Halobiz.Common.DTOs.ReceivingDTOs
{
    public class AuthUserProfileReceivingDTO
    {
        public string IdToken { get; set; }
        public UserProfileReceivingDTO UserProfile { get; set; }
    }
}