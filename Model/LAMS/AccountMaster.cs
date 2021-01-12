using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HaloBiz.Model.ManyToManyRelationship;

namespace HaloBiz.Model.AccountsModel
{
    public class AccountMaster
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        [Required]
        public long AccountMasterAlias { get; set; }
        [Required]
        public bool IntegrationFlag { get; set; }
        [Required]
        public long VoucherId { get; set; }
        [Required]
        public long ChartofAccountSubId { get; set; }
        public virtual Account ChartofAccountSub { get; set; }
        [Required]
        public long BranchId { get; set; }
        public virtual Branch Branch { get; set; }
        [Required]
        public long OfficeId { get; set; }
        public virtual Office Office { get; set; }
        public IEnumerable<SBUAccountMaster> SBUAccountMaster { get; set; }
        [Required]
        public long CreatedById { get; set; }
        [Required]
        public virtual UserProfile CreatedBy { get; set; }
        [Required]
        public bool IsDeleted { get; set; } = false;
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }
    }
}