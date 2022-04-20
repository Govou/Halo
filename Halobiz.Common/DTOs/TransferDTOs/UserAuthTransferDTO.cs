using System;

namespace Halobiz.Common.DTOs.TransferDTOs
{
    public class UserAuthTransferDTO
    {
        public string Token { get; set; }
        public double JwtLifespan { get; set; } //in minutes
        public string RefreshToken { get; set; }
        public UserProfileTransferDTO UserProfile { get; set; }
        public string[] Roles { get; set; } = new string[] { };
    }
}