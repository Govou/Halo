namespace HaloBiz.DTOs.TransferDTOs.LAMS
{
    public class LeadKeyPersonTransferDTO : ContactTransferDTO
    {
        public long LeadId { get; set; }
        public virtual LeadWithoutModelsTransferDTO Lead { get; set; }
    }
}