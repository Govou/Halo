using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HaloBiz.Model.LAMS;

namespace HaloBiz.Model.AccountsModel
{
    public class Invoice
    {
        [Key]
        public long Id { get; set; }
        public string InvoiceNumber { get; set; }
        public long UnitPrice { get; set; }
        public int Quantity { get; set; }
        public double Discount { get; set; }
        public double Value { get; set; }
        public DateTime DateToBeSent { get; set; }
        public bool IsInvoiceSent { get; set; }
        public long CustomerDivisionId { get; set; }
        public CustomerDivision CustomerDivision { get; set; }
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

/*InvoiceNumber
Unit
Quantity
Discount
Value
DateToBeSent
IsInvoiceSent
ContractId
ContractServiceId
CustomerDivisionId
*/