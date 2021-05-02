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
        public long CustomerDivisionId { get; set; }
        public long? ContractId { get; set; }
        public long? ContractServiceId { get; set; }
        public CustomerDivision CustomerDivision { get; set; }
        public Contract Contract { get; set; }
        public ContractService ContractService { get; set; }
    }
}
