using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaloBiz.Model
{
    public class ModificationHistory
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public string ModelChanged { get; set; }
        public long ModifiedModelId { get; set; }
        public long ChangedById { get; set; }
        public UserProfile ChangedBy { get; set; }
        [StringLength(20000)]
        public string ChangeSummary { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }
    }
}