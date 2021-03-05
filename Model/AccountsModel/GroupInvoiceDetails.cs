using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HaloBiz.Model.LAMS;
using System;

namespace HaloBiz.Model.AccountsModel
{
    public class GroupInvoiceDetails
    {
        [Key]
        public long Id { get; set; }
        public string InvoiceNumber { get; set; }
        public string Description { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }
        public double  VAT { get; set; }
        public double Value { get; set; }
        public double BillableAmount { get; set; }
        public long ContractServiceId { get; set; }
        public ContractService ContractService { get; set; }
        public bool IsDeleted { get; set; } = false;
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }
        public long? CreatedById { get; set; }
        public UserProfile CreatedBy { get; set; }
           
    }
}