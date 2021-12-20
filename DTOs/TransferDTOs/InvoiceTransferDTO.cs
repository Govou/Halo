using System;
using System.Collections.Generic;
using halobiz_backend.Helpers;
using HalobizMigrations.Models;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class InvoiceTransferDTO
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
        public bool IsInvoiceSent { get; set; }
        public long CustomerDivisionId { get; set; }
        public InvoiceStatus IsReceiptedStatus { get; set; }
        public bool IsReversed { get; set; }
        public bool IsReversalInvoice { get; set; }
        public double TotalAmountReceipted { get; set; }
        public IEnumerable<Receipt> Receipts { get; set; }
        public long ContractId { get; set; }
        public long ContractServiceId { get; set; }
        public bool IsFinalInvoice { get; set; }

        public double ToReceiptValue { get; set; }
        public double TotalReceiptApplied { get; set; }
    }
}