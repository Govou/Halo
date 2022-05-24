namespace HaloBiz.DTOs.TransferDTOs
{
    public class ApproverLevelTransferDTO : BaseSetupTransferDTO
    {
        public string Alias { get; set; }
    }

    public class ApprovingLevelOfficeTransferDTO
    {
        public long Id { get; set; }
        public long? UserId { get; set; }
        public Halobiz.Common.DTOs.TransferDTOs.UserProfileTransferDTO User { get; set; }
        public System.Collections.Generic.ICollection<ApprovingLevelOfficerTransferDTO> ApprovingOfficers { get; set; }
        public long CreatedById { get; set; }
        public bool IsDeleted { get; set; }
        public System.DateTime CreatedAt { get; set; }
    }

    public class ApprovingLevelOfficerTransferDTO
    {
        public long Id { get; set; }
        public long? ApprovingLevelOfficeId { get; set; }
        public ApprovingLevelOfficeTransferDTO ApprovingLevelOffice { get; set; }
        public long? UserId { get; set; }
        public Halobiz.Common.DTOs.TransferDTOs.UserProfileTransferDTO User { get; set; }
        public long CreatedById { get; set; }
        public bool IsDeleted { get; set; }
        public System.DateTime CreatedAt { get; set; }
    }
}