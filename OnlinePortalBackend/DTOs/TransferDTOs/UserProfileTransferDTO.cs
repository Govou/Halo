using System;

namespace OnlinePortalBackend.DTOs.TransferDTOs
{
    public class UserProfileTransferDTO
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OtherName { get; set; }
        public string CodeName { get; set; }
        public string DateOfBirth { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string ImageUrl { get; set; }
        public string AltEmail { get; set; }
        public string AltMobileNumber { get; set; }
        public string Address { get; set; }
    }
}