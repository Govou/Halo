using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaloBiz.Model
{
    public class Division
    {
        [Key]
        public long Id { get; set; }
        [Required, MinLength(3), MaxLength(100)]
        public string Name { get; set; }
        [Required, MinLength(3), MaxLength(255)]
        public string Description { get; set; }
        public string MissionStatement { get; set; }
        public long HeadId { get; set; }
        public virtual UserProfile Head { get; set; }
        public IEnumerable<OperatingEntity> OperatingEntities { get; set; }
        public bool IsDeleted { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }
    }
}