using HaloBiz.Model.LAMS;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaloBiz.Model
{
    public class ServiceCategoryTask
    {
        public long Id { get; set; }
        [Required]
        public string Caption { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        public long ServiceCategoryId { get; set; }
        public virtual ServiceCategory ServiceCategory { get; set; }
        public IEnumerable<ServiceTaskDeliverable> ServiceTaskDeliverable { get; set; }
        public long CreatedById { get; set; }
        public virtual UserProfile CreatedBy { get; set; }
        public long? EndorsementTypeId { get; set; }
        public EndorsementType EndorsementType { get; set; }
        public bool IsDeleted { get; set; } = false;
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }
    }
}