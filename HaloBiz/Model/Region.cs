using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaloBiz.Model
{
    public class Region
    {
        [Key]
        public long Id { get; set; }
        [Required, MinLength(3), MaxLength(50)]
        public string Name { get; set; }
        [Required, MinLength(3), MaxLength(255)]
        public string Description { get; set; }
        [Required, MaxLength(500, ErrorMessage="Requires a maximum of 500 characters")]
        public long BranchId { get; set; }
        public Branch Branch { get; set; }
        public long HeadId { get; set; }
        public virtual UserProfile Head { get; set; }
        public IEnumerable<Zone> Zones { get; set; }
        
        public long CreatedById { get; set; }
        [Required]
        public virtual UserProfile CreatedBy { get; set; }
        [Required]
        public bool IsDeleted { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }
    }
}
