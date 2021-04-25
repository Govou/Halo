using HalobizMigrations.Models;
using System.Collections.Generic;


namespace HaloBiz.DTOs.TransferDTOs.LAMS
{
    public class ClosureDocumentTransferDTO : DocumentSetupTransferDTO
    {
        public long ContractServiceId { get; set; }
        public ContractService ContractService { get; set; }
    }
}