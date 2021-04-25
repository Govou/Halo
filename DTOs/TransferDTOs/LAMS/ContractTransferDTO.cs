using System.Collections.Generic;
using HalobizMigrations.Models;


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
        public IEnumerable<ContractServiceTransferDTO> ContractService { get; set; }
    }

     public class ContractForCustomerDivisionTransferDTO
    {
        public long Id { get; set; }
        public string ReferenceNo { get; set; }
        public long CustomerDivisionId { get; set; }
        public double AmountPaid { get; set; } = 0;
        public IEnumerable<ContractServiceForContractTransferDTO> ContractService { get; set; }
    }

    public class ContractSummaryTransferDTO : ContractForCustomerDivisionTransferDTO
    {
        public double ContractValue { get; set; }
    }
}