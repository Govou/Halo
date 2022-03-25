using HalobizMigrations.Models;
using HalobizMigrations.Models.OnlinePortal;
using System;

namespace OnlinePortalBackend.DTOs.TransferDTOs
{
    public class CartContractDetailDTO
    {
        public long Id { get; set; }
        public double UnitPrice { get; set; }
        public long? InvoiceCycleInDays { get; set; }
        public long? BranchId { get; set; }
        public long? OfficeId { get; set; }
        public double AdHocInvoicedAmount { get; set; }
        public string UniqueTag { get; set; }
        public string AdminDirectTie { get; set; }
        public long Quantity { get; set; }
        public double Discount { get; set; }
        public double? Vat { get; set; }
        public double? BillableAmount { get; set; }
        public double? Budget { get; set; }
        public int? PaymentCycle { get; set; }
        public int? InvoicingInterval { get; set; }
        public long ServiceId { get; set; }
        public DateTime? ContractEndDate { get; set; }
        public DateTime? ContractStartDate { get; set; }
        public DateTime? FirstInvoiceSendDate { get; set; }
    }
}
