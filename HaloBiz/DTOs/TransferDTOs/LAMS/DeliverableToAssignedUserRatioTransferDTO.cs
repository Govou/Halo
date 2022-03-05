using HalobizMigrations.Models;

namespace halobiz_backend.DTOs.TransferDTOs.LAMS
{
    public class DeliverableToAssignedUserRatioTransferDTO
    {
        public UserProfile User { get; set; }
        public long Proportion { get; set; }
    }
}