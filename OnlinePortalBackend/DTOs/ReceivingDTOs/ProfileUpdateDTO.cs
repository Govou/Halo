using System;

namespace OnlinePortalBackend.DTOs.ReceivingDTOs
{
    public class ProfileUpdateDTO
    {
        public int profileId { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string ProfileImage { get; set; }
    }
}
