using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaloBiz.Model.LAMS
{
    public class LeadOrigin
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public string Caption { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        [Required]
        public long LeadTypeId { get; set; }
        [Required]
        public virtual LeadType LeadType { get; set; }
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