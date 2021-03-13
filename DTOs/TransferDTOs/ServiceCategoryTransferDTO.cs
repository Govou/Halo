using System.Collections.Generic;
using HaloBiz.Model;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class ServiceCategoryTransferDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long ServiceGroupId { get; set; }
        public ServiceGroupWithoutServiceCategoryDTO ServiceGroup { get; set; }
        public long OperatingEntityId { get; set; }
        public long DivisionId { get; set; }
        public IEnumerable<ServicesTransferDTO> Services { get; set; }
        public IEnumerable<ServiceCategoryTaskTransferDTO> ServiceCategoryTasks { get; set; }
    }
}