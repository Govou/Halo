using System;

namespace OnlinePortalBackend.DTOs.ReceivingDTOs
{
    public class ProfileUpdateDTO
    {
        public int profileId { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string ProfileImage { get; set; }
        public int? StateId { get; set; }
        public string StateName { get; set; }
        public int? LGAId { get; set; }
        public string LGAName { get; set; }
        public string Street { get; set; }

    }
}


