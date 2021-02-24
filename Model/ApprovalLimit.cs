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
        public string ModuleCaptured { get; set; }
        [Required]
        public long UpperlimitValue { get; set; }
        [Required]
        public long LowerlimitValue { get; set; }
        [Required]
        public string ApproverLevel { get; set; }
        [Required]
        public string Sequence { get; set; }
        public bool IsDeleted { get; set; } = false;
        [Required]
        public bool IsBypassRequired { get; set; } = false;

       [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }
    }
}
