using System;

using halobiz_backend.Helpers;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class InvoiceReceivingDTO
    {
        public double UnitPrice { get; set; }
        public long Quantity { get; set; }
        public double Discount { get; set; }
        public double Value { get; set; }
        public double VAT { get; set; }
        public double BillableAmount { get; set; }
        public DateTime DateToBeSent { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long CustomerDivisionId { get; set; }
        public long ContractServiceId { get; set; }
    }
}