using HaloBiz.Model;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class ApprovalLimitTransferDTO : BaseSetupTransferDTO
    {
        public long ProcessesRequiringApprovalId { get; set; } //module captured
        public virtual ProcessesRequiringApproval ProcessesRequiringApproval { get; set; }
        public long UpperlimitValue { get; set; }
        public long LowerlimitValue { get; set; }
        public long ApproverLevelId { get; set; }
        public virtual ApproverLevel ApproverLevel { get; set; }
        public long Sequence { get; set; }
        public bool IsBypassRequired { get; set; } = false;

    }
}