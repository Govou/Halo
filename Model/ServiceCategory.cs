using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaloBiz.Model
{
    public class ServiceCategory
    {
        [Key]
        public long Id { get; set; }
        [Required, MinLength(3), MaxLength(50)]
        public string Name { get; set; }
        [Required, MinLength(3), MaxLength(255)]
        public string Description { get; set; }
        [Required]
        public long ServiceGroupId { get; set; }
        public virtual ServiceGroup ServiceGroup { get; set; }
        [Required]
        public long OperatingEntityId { get; set; }
        [Required]
        public long DivisionId { get; set; }
        public IEnumerable<Services> Services { get; set; }
        public IEnumerable<ServiceCategoryTask> ServiceCategoryTasks { get; set; }
        public bool IsDeleted { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }
    }
}