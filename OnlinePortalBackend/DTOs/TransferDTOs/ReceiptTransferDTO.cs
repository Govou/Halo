using System;

namespace OnlinePortalBackend.DTOs.TransferDTOs
{
    public class ReceiptTransferDTO
    {
        public long Id { get; set; }
        public string Caption { get; set; }
        public string InvoiceNumber { get; set; }
        public double InvoiceValue { get; set; }
        public string TransactionId { get; set; }
        public double InvoiceValueBalanceBeforeReceipting { get; set; }
        public DateTime DateAndTimeOfFundsReceived { get; set; }
        public double ReceiptValue { get; set; }
        public string Depositor { get; set; }
        public double InvoiceValueBalanceAfterReceipting { get; set; }
        public bool IsTaskWitheld { get; set; }
        public double ValueOfWHT { get; set; }
        public string ReceiptNumber { get; set; }
        public long InvoiceId { get; set; }
        public bool IsReversed { get; set; }
        public bool IsReversalReceipt { get; set; }

        public string EvidenceOfPaymentUrl { get; set; }
    }
}
