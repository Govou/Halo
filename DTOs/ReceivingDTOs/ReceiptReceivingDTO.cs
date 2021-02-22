using System;
using System.ComponentModel.DataAnnotations;

namespace halobiz_backend.DTOs.ReceivingDTOs
{
    public class ReceiptReceivingDTO
    {
        public string Caption { get; set; }
        public double InvoiceValue { get; set; }
        public double InvoiceValueBalanceBeforeReceipting { get; set; }
        public DateTime DateAndTimeOfFundsReceived { get; set; }
        public double ReceiptValue { get; set; }
        public string Depositor { get; set; }
        public double InvoiceValueBalanceAfterReceipting { get; set; }
        public bool IsTaskWitheld { get; set; }
        public double ValueOfWHT { get; set; }
        public long InvoiceId { get; set; }
        public long ClientId { get; set; }
        public long BankAccountId { get; set; }
        [StringLength(2000)]
        public string EvidenceOfPaymentUrl { get; set; }
    }
}