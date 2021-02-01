using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaloBiz.Model.LAMS
{
    public class LeadDivision
    {
        [Key]
        public long Id { get; set; }
        [StringLength(50)]
        public long LeadOriginId { get; set; }
        public virtual LeadOrigin LeadOrigin { get; set; }
        [StringLength(100)]
        public string Industry { get; set; }
        [StringLength(50)]
        public string RCNumber { get; set; }
        [StringLength(250)]
        public string DivisionName { get; set; }
        [Required, RegularExpression("\\d{10,15}")]
        public string PhoneNumber { get; set; }
        [Required, RegularExpression("^[_A-Za-z0-9-\\+]+(\\.[_A-Za-z0-9-]+)*@[A-Za-z0-9-]+(\\.[A-Za-z0-9]+)*(\\.[A-Za-z]{2,})$", ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [StringLength(1000)]
        public string LogoUrl { get; set; }
        public long? PrimaryContactId { get; set; }
        public virtual LeadDivisionContact PrimaryContact { get; set; }
        public long? SecondaryContactId { get; set; }
        public LeadDivisionContact SecondaryContact { get; set; }
        public long? BranchId { get; set; }
        public virtual Branch Branch { get; set; }
        public long? OfficeId { get; set; }
        public virtual Office Office { get; set; }
        public long LeadId { get; set; }
        public virtual Lead Lead { get; set; }
        public  Quote Quote { get; set; }
        public IEnumerable<LeadDivisionKeyPerson> LeadDivisionKeyPersons { get; set; }
        public long CreatedById { get; set; }
        public virtual UserProfile CreatedBy { get; set; }
        public bool IsDeleted { get; set; } = false;
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }
    }
}

