using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaloBiz.Model
{
    public class Zone
    {
        [Key]
        public long Id { get; set; }
        [Required, MinLength(3), MaxLength(50)]
        public string Name { get; set; }
        [Required, MinLength(3), MaxLength(255)]
        public string Description { get; set; }
        public long HeadId { get; set; }
        public virtual UserProfile Head { get; set; }
        public long StateId { get; set; }
        public State State { get; set; }
        public long LGAId { get; set; }
        public LGA LGA  { get; set; }
        public long RegionId { get; set; }
        public Region Region { get; set; }
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

