using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Model.LAMS
{
    public class EndorsementType
    {
        [Key]
        public long Id { get; set; }
        [Required, MinLength(1), MaxLength(255)]
        public string Caption { get; set; }
        [Required, MinLength(1), MaxLength(255)]
        public string Description { get; set; }
        [Required]
        public long CreatedById { get; set; }
        public bool IsDeleted { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }
    }
}
