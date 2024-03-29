using Halobiz.Common.Model;
using System;

namespace Halobiz.Common.DTOs.ReceivingDTOs
{
    public class ReceiptReceivingDTO
    {
        public string Caption { get; set; }
        public string InvoiceNumber { get; set; }
        public double InvoiceValue { get; set; }
        public double InvoiceValueBalanceBeforeReceipting { get; set; }
        public DateTime DateAndTimeOfFundsReceived { get; set; } = DateTime.Now;
        public double ReceiptValue { get; set; }
        public string Depositor { get; set; }
        public double InvoiceValueBalanceAfterReceipting { get; set; }
        public bool IsTaskWitheld { get; set; }
        public double ValueOfWHT { get; set; }
        public long InvoiceId { get; set; }
        public long AccountId { get; set; }
        public string EvidenceOfPaymentUrl { get; set; }
        public int PaymentGateway { get; set; }
        public string PaymentReference { get; set; }
        public long? CreditNoteOrAdvancePaymentId { get; set; }
        public ReceiptPostingSourceOfFund Source { get; set; } = ReceiptPostingSourceOfFund.Bank;

    }

    public class SMSReceiptReceivingDTO
    {

        public string Caption { get; set; }
        public string InvoiceNumber { get; set; }
        public double InvoiceValue { get; set; }
        public long InvoiceId { get; set; }
        public int PaymentGateway { get; set; }
        public string PaymentReference { get; set; }
        public long? AdHocIvoiceGroup { get; set; }
    }

    public class SMSReceiptInvoiceForContractDTO
    {

        public string Caption { get; set; }
        public long ContractId { get; set; }
        public int[] ContractServices { get; set; }
        public double InvoiceValue { get; set; }
        public int PaymentGateway { get; set; }
        public string PaymentReference { get; set; }
    }
}