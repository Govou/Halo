using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaloBiz.Model
{
    public class LGA
    {
        [Key]
        public long Id { get; set; }
        [Required, MaxLength(100)]
        public string Name { get; set; }
        [Required]
        public long StateId { get; set; }
        public virtual State State { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }
    }
}