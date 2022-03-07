using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HaloBiz.Model.AccountsModel;

namespace HaloBiz.Model.LAMS
{
    public class CustomerDivision
    {
        [Key]
        public long Id { get; set; }
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
        [StringLength(5000)]
        public string LogoUrl { get; set; }
        [StringLength(1000)]
        public string Address { get; set; }
        public long CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public long? AccountId { get; set; }
        public Account Account { get; set; }
        public IEnumerable<AccountMaster> AccountMaster { get; set; }
        public IEnumerable<Contract> Contracts { get; set; }
        public OtherLeadCaptureInfo OtherLeadCaptureInfo { get; set; }
        public IEnumerable<TaskFulfillment> TaskFulfillments { get; set; }
        public long CreatedById { get; set; }
        public List<LeadKeyPerson> LeadKeyPeople { get; set; } 
        public LeadDivisionContact PrimaryContact { get; set; }
        public long? PrimaryContactId { get; set; }   
        public LeadDivisionContact SecondaryContact { get; set; }
        public long? SecondaryContactId { get; set; }
        #region Address is a concatenation of State, LGA and Street for each lead Division. Address will be auto populated
        public long? StateId { get; set; }
        public virtual State State { get; set; }
        public long? LGAId { get; set; }
        public LGA LGA { get; set; }
        public string Street { get; set; }
        #endregion
        public virtual UserProfile CreatedBy { get; set; }
        public bool IsDeleted { get; set; } = false;
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }
    }
}
