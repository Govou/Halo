using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace HaloBiz.Model

{
    public class SupplierCategory
    {
        [Key]
        public long Id { get; set; }
        [Required, MinLength(2), MaxLength(50)]
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public long CreatedById { get; set; }
        public UserProfile CreatedBy { get; set; }
        public bool IsDeleted { get; set; } = false;
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }
    }
}
