using System;

namespace OnlinePortalBackend.DTOs.ReceivingDTOs
{
    public class ProfileUpdateDTO
    {
        public int profileId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfileImage { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
