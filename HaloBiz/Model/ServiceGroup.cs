using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaloBiz.Model
{
    public class ServiceGroup
    {
        [Key]
        public long Id { get; set; }
        [Required, MinLength(3), MaxLength(50)]
        public string Name { get; set; }
        [Required, MinLength(3), MaxLength(255)]
        public string Description { get; set; }
        [Required]
        public long OperatingEntityId { get; set; }
        public virtual OperatingEntity OperatingEntity { get; set; }
        [Required]
        public long DivisionId { get; set; }
        public IEnumerable<ServiceCategory> ServiceCategories { get; set; }
        public bool IsDeleted { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }
    }
}