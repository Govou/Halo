namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class ApprovalLimitReceivingDTO : BaseSetupDTO
    {
        public long ProcessesRequiringApprovalId { get; set; } //module captured
        public long UpperlimitValue { get; set; }
        public long LowerlimitValue { get; set; }
        public long ApproverLevelId { get; set; }
        public long Sequence { get; set; }
        public bool IsBypassRequired { get; set; } = false;
    }
}