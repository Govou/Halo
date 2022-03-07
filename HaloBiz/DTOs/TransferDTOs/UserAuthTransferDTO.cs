namespace HaloBiz.DTOs.TransferDTOs
{
    public class UserAuthTransferDTO
    {
        public string Token { get; set; }
        public UserProfileTransferDTO UserProfile { get; set; }
    }
}