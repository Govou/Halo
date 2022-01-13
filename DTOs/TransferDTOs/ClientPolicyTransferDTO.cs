using HalobizMigrations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class ClientPolicyTransferDTO
    {
        public long Id { get; set; }
        public bool AutoRenew { get; set; }
        public long? RateReviewInterval { get; set; }
        public DateTime? NextRateReviewDate { get; set; }
        public long? ContractServiceId { get; set; }
        public ContractService ContractService { get; set; }
        public int? RenewalTenor { get; set; }
    }
}
