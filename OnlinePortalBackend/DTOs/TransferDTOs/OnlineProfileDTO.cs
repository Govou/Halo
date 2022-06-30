using System;
using System.Collections.Generic;

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
        public int StateId { get; set; }
        public string StateName { get; set; }
        public int LGAId { get; set; }
        public string LGAName { get; set; }
        public string Street { get; set; }

        public List<OnlineContact> Contacts { get; set; }

    }

    public class OnlineContact
    {
        public string ContactType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string ProfilePicture { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }

    }
}

