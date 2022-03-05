using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HaloBiz.Model;

namespace halobiz_backend.Model
{
    public class TaskEscalationTiming
    {
        [Key]
        public long Id { get; set; }
        public long Duration { get; set; }
        public string Module { get; set; }
        public string Stage { get; set; }
        public long ServiceCategoryId { get; set; }
        public ServiceCategory ServiceCategory { get; set; }
        public string Value { get; set; }
        public bool IsDeleted { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }
    }
}
