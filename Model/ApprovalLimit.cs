using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Model
{
    public class ApprovalLimit
    {
        [Key]
        public long Id { get; set; }
        [Required, MinLength(3), MaxLength(50)]
        public string Caption { get; set; }
        [Required, MinLength(3), MaxLength(255)]
        public string Description { get; set; }
        [Required]

        public long ProcessesRequiringApprovalId { get; set; } //module captured
        public virtual ProcessesRequiringApproval ProcessesRequiringApproval { get; set; }
        public string ModuleCaptured { get; set; }
        [Required]
        public long UpperlimitValue { get; set; }
        [Required]
        public long LowerlimitValue { get; set; }
        [Required]
        public long ApproverLevelId { get; set; }
        public virtual ApproverLevel ApproverLevel { get; set; }
        [Required]
        public long Sequence { get; set; }
        public bool IsDeleted { get; set; } = false;
        public long CreatedById { get; set; }
        public virtual UserProfile CreatedBy { get; set; }

        [Required]
        public bool IsBypassRequired { get; set; } = false;

       [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }
    }
}
