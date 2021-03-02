using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HaloBiz.Helpers;
using HaloBiz.Model.LAMS;
using halobiz_backend.Helpers;
using halobiz_backend.Model.AccountsModel;

namespace HaloBiz.Model.AccountsModel
{
    public class Invoice
    {
        [Key]
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
        public CustomerDivision CustomerDivision { get; set; }
        public InvoiceStatus IsReceiptedStatus { get; set; }
        public bool IsFinalInvoice { get; set; }= true;
        public InvoiceType InvoiceType { get; set; } = InvoiceType.New;
        public IEnumerable<Receipt> Receipts { get; set; }
        [Required]
        public long ContractId { get; set; }
        public Contract Contract { get; set; }
        [Required]
        public long ContractServiceId { get; set; }
        public ContractService ContractService { get; set; }
        public bool IsDeleted { get; set; } = false;
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }

    }
}
