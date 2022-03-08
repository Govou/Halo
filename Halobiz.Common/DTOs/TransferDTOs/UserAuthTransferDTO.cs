namespace Halobiz.Common.DTOs.TransferDTOs
{
    public class UserAuthTransferDTO
    {
        public string Token { get; set; }
        public UserProfileTransferDTO UserProfile { get; set; }
    }
}