using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaloBiz.Model.AccountsModel
{
    public class ControlAccount
    {
        [Key]
        public long Id { get; set; }
        [StringLength(100)]
        public string Caption { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        [StringLength(100)]
        public string Alias { get; set; }
        public long AccountClassId { get; set; }
        public AccountClass AccountClass { get; set; }
        public IEnumerable<Account> Accounts { get; set; }
        public long CreatedById { get; set; }
        public virtual UserProfile CreatedBy { get; set; }
        public bool IsDeleted { get; set; } = false;
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }

    }
}

