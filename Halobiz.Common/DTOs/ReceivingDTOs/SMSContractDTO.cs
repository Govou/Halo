using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halobiz.Common.DTOs.ReceivingDTOs
{
    public class SMSContractDTO
    {
       
      
        public DateTime ContractStartDate { get; set; }
        public DateTime ContractEndDate { get; set; }
        public DateTime FirstInvoiceSendDate { get; set; }
        public int InvoicingInterval { get; set; }
        public DateTime ActivationDate { get; set; }
        public DateTime FulfillmentStartDate { get; set; }
        public DateTime FulfillmentEndDate { get; set; }
        public int OfficeId { get; set; }
        public int BranchId { get; set; }
        public string Email { get; set; }
        public List<SMSContractService> SMSContractServices { get; set; }
    }

    public class SMSContractService
    {
        public int ServiceId { get; set; }
        public double Vat { get; set; }
        public double BillableAmount { get; set; }
        public double? UnitPrice { get; set; }
        public int Quantity { get; set; }
        public double Discount { get; set; }
        public string UniqueTag { get; set; }
        public int PaymentCycle { get; set; }
        public int ContractId { get; set; }
    }
}
