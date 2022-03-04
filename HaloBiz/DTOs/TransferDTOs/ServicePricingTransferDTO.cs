using HalobizMigrations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class ServicePricingTransferDTO
    {
        public long Id { get; set; }
        public long ServiceId { get; set; }
        public decimal MinimumAmount { get; set; }
        public decimal MaximumAmount { get; set; }
        public long BranchId { get; set; }
        public long CreatedById { get; set; }
        public UserProfile CreatedBy { get; set; }
        public Service Service { get; set; }
        public Branch Branch { get; set; }
    }
}
