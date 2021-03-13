using System.Collections.Generic;
using HaloBiz.Model.LAMS;

namespace HaloBiz.DTOs.TransferDTOs.LAMS
{
    public class ClosureDocumentTransferDTO : DocumentSetupTransferDTO
    {
        public long ContractServiceId { get; set; }
        public ContractService ContractService { get; set; }
    }
}