using System;

namespace Halobiz.Common.DTOs.TransferDTOs
{
    public class UserAuthTransferDTO
    {
        public string Token { get; set; }
        public DateTime TokenExpiryTime { get; set; }
        public string RefreshToken { get; set; }
        public UserProfileTransferDTO UserProfile { get; set; }
    }
}