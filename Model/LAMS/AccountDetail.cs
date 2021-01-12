using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaloBiz.Model.AccountsModel
{
    public class AccountDetail
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        [Required]
        public long AccountDetailsAlias { get; set; }
        [Required]
        public bool IntegrationFlag { get; set; }
        [Required]
        public long VoucherId { get; set; }
        [Required]
        public long TransactionId { get; set; }
        [Required]
        public DateTime TransactionDate { get; set; }
        [Required]
        public double Credit { get; set; }
        [Required]
        public double Debit { get; set; }
        [Required]
        public long BranchId { get; set; }
        public virtual Branch Branch { get; set; }
        [Required]
        public long OfficeId { get; set; }
        public virtual Office Office { get; set; }
        [Required]
        public long AccountMasterId { get; set; }
        [Required]
        public virtual AccountMaster AccountMaster { get; set; }
        public long AccountClassAlias { get; set; }
        public long CreatedById { get; set; }
        public virtual UserProfile CreatedBy { get; set; }
        public bool IsDeleted { get; set; } = false;
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }
    }
}