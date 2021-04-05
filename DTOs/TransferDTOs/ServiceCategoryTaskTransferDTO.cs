using System.Collections.Generic;
using HaloBiz.DTOs.TransferDTOs.LAMS;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class ServiceCategoryTaskTransferDTO
    {
        public long Id { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }
        public virtual ServiceCategoryWithoutServicesTransferDTO ServiceCategory { get; set; }
        public IEnumerable<BaseSetupTransferDTO> ServiceTaskDeliverable { get; set; }
        public long? EndorsementTypeId { get; set; }
        public EndorsementTypeTransferDTO EndorsementType { get; set; }
    }
}