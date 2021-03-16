using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaloBiz.Model.AccountsModel
{
    public class Account
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public bool IntegrationFlag { get; set; } = false;
        [StringLength(1000)]
        public string Description { get; set; }
        [Required]
        public string Alias { get; set; }
        [Required]
        public bool IsDebitBalance { get; set; }
        public bool IsActive { get; set; } = true;
        [Required]
        public long? ControlAccountId { get; set; }
        public virtual ControlAccount ControlAccount { get; set; }
        public  IEnumerable<AccountDetail> AccountDetails { get; set; }
        public long CreatedById { get; set; }
        public virtual UserProfile CreatedBy { get; set; }
        public bool IsDeleted { get; set; } = false;
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }
    }
}