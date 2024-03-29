using System.Collections.Generic;
using Halobiz.Common.DTOs.TransferDTOs;
using HalobizMigrations.Models;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class ServiceGroupTransferDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public OperatingEntityWithoutServiceGroupDTO OperatingEntity { get; set; }
        public long DivisionId { get; set; }
        public IEnumerable<ServiceCategoryTransferDTO> ServiceCategories { get; set; }

    }
}