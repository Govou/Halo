using System;

namespace OnlinePortalBackend.DTOs.TransferDTOs
{
    public class OnlineProfileDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public string profileImage { get; set; }
        public string PercentageCompletion { get; set; }

    }
}

