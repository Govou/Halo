namespace HaloBiz.DTOs.TransferDTOs
{
    public class StandardSLAForOperatingEntitiesTransferDTO : BaseSetupTransferDTO
    {
        public string DocumentUrl { get; set; }
        public virtual OperatingEntityWithoutServiceGroupDTO OperatingEntity { get; set; }
    }
}