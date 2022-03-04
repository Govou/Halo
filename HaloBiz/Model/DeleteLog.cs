using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaloBiz.Model
{
    public class DeleteLog
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public string DeletedModel { get; set; }
        public long DeletedModelId { get; set; }
        public long DeletedById { get; set; }
        public UserProfile DeletedBy { get; set; }
        [StringLength(20000)]
        public string ChangeSummary { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }
    }
}