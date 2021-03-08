namespace HaloBiz.DTOs.TransferDTOs
{
    public class SBUProportionTransferDTO
    {
        public long Id { get; set; }
        public long OperatingEntityId { get; set; }
        public OperatingEntityTransferDTO OperatingEntity { get; set; }
        public int LeadClosureProportion { get; set; }
        public int LeadGenerationProportion { get; set; }
    }
}