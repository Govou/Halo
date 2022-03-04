using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaloBiz.Model
{
    public class Bank
    {
        [Key]
        public long Id { get; set; }
        [Required, StringLength(200)]
        public string Name { get; set; }
        [StringLength(500)]
        public string Slogan { get; set; }
        [StringLength(200)]
        public string Alias { get; set; }
        [Required]
        public bool IsActive { get; set; } = true;
        [Required]
        public long CreatedById { get; set; }
        public virtual UserProfile CreatedBy { get; set; }
        public bool IsDeleted { get; set; } = false;
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }
    }
}