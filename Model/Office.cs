using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HaloBiz.Model.LAMS;

namespace HaloBiz.Model
{
    public class Office
    {
        [Key]
        public long Id { get; set; }
        [Required, MinLength(3), MaxLength(50)]
        public string Name { get; set; }
        [Required, MinLength(3), MaxLength(255)]
        public string Description { get; set; }
        public long StateId { get; set; }
        public virtual State State { get; set; }
        public long LGAId { get; set; }
        public LGA LGA { get; set; }
        [Required]
        public string Street { get; set; }
        public string PhoneNumber { get; set; }
        public long HeadId { get; set; }
        public UserProfile Head { get; set; }
        public long BranchId { get; set; }
        public virtual Branch Branch { get; set; }
        public bool IsDeleted { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }
    }
}