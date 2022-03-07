using HaloBiz.DTOs.ReceivingDTO;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class AuthUserProfileReceivingDTO
    {
        public string IdToken { get; set; }
        public UserProfileReceivingDTO UserProfile { get; set; }
    }
}