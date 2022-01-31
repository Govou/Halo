namespace HaloBiz.DTOs.TransferDTOs
{
    public class StandardSlaforOperatingEntityTransferDTO : BaseSetupTransferDTO
    {
        public string DocumentUrl { get; set; }
        public virtual OperatingEntityWithoutServiceGroupDTO OperatingEntity { get; set; }
    }
}