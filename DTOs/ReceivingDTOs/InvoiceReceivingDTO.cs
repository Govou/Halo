using System;
using HaloBiz.Model.LAMS;
using halobiz_backend.Helpers;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class InvoiceReceivingDTO
    {
        public long Id { get; set; }
        public string InvoiceNumber { get; set; }
        public string TransactionId { get; set; }
        public double UnitPrice { get; set; }
        public long Quantity { get; set; }
        public double Discount { get; set; }
        public double Value { get; set; }
        public DateTime DateToBeSent { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long CustomerDivisionId { get; set; }
        public InvoiceStatus IsReceiptedStatus { get; set; }
        public long ContractId { get; set; }
        public long ContractServiceId { get; set; }
    }
}