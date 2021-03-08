using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaloBiz.Model
{
    public class SBUProportion
    {
        [Key]
        public long Id { get; set; }
        public long OperatingEntityId { get; set; }
        public OperatingEntity OperatingEntity { get; set; }
        public int LeadClosureProportion { get; set; }
        public int LeadGenerationProportion { get; set; }
        public long CreatedById { get; set; }
        public virtual UserProfile CreatedBy { get; set; }
        public bool IsDeleted { get; set; }=false;
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }
    }
}