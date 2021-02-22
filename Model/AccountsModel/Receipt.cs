using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HaloBiz.Model;
using HaloBiz.Model.AccountsModel;

namespace halobiz_backend.Model.AccountsModel
{
    public class Receipt
    {
        [Key]
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
        public Invoice Invoice { get; set; }
        [StringLength(2000)]
        public string EvidenceOfPaymentUrl { get; set; }
        public long CreatedById { get; set; }
        public UserProfile CreatedBy { get; set; }public bool IsDeleted { get; set; } = false;
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }
    }
}