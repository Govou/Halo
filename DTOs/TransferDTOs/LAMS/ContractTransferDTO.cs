using System.Collections.Generic;
using HaloBiz.Model;
using HaloBiz.Model.LAMS;

namespace HaloBiz.DTOs.TransferDTOs.LAMS
{
    public class ContractTransferDTO
    {
        public long Id { get; set; }
        public string ReferenceNo { get; set; }
        public long CustomerDivisionId { get; set; }
        public CustomerDivision CustomerDivision { get; set; }
        public long QuoteId { get; set; }
        public virtual Quote Quote { get; set; }
        public IEnumerable<ContractServiceTransferDTO> ContractServices { get; set; }
    }

     public class ContractForCustomerDivisionTransferDTO
    {
        public long Id { get; set; }
        public string ReferenceNo { get; set; }
        public long CustomerDivisionId { get; set; }
        public IEnumerable<ContractServiceForContractTransferDTO> ContractServices { get; set; }
    }
}