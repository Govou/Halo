using System;
using System.Collections.Generic;

namespace HaloBiz.DTOs.ReceivingDTOs
{
        public class GroupInvoiceDto
        {
            public double TotalBillable { get; set; }
            public double VAT { get; set; }
            public string GroupInvoiceNumber { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public DateTime DateToBeSent { get; set; }
            public long LeadContractServiceId { get; set; }
            public long CustomerDivisionId { get; set; }
            public List<ContractServiceToAmount> ContractServiceToAmount { get; set; }
        }

        public class ContractServiceToAmount
        {
            public long ContractServiceId { get; set; }
            public double BillableAmount { get; set; }
            public double VAT { get; set; }
        }
}