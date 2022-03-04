using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HaloBiz.Model.LAMS
{
    public class Lead
    {
        [Key]
        public long Id { get; set; }
        [StringLength(50)]
        public string ReferenceNo { get; set; }
        public long LeadTypeId { get; set; }
        public virtual LeadType LeadType { get; set; }
        public long LeadOriginId { get; set; }
        public virtual LeadOrigin LeadOrigin { get; set; }
        [StringLength(100)]
        public string Industry { get; set; }
        [StringLength(50)]
        public string RCNumber { get; set; }
        [StringLength(250)]
        public string GroupName { get; set; }
        public long GroupTypeId { get; set; }
        public GroupType GroupType { get; set; }
        [StringLength(5000)]
        public string LogoUrl { get; set; }
        public string LeadCaptureDocumentUrl { get; set; }
        public bool LeadCaptureStatus { get; set; } = false;
        public bool LeadQualificationStatus { get; set; } = false;
        public DateTime? TimeMovedToLeadQualification { get; set; }
        public bool LeadOpportunityStatus { get; set; } = false;
        public DateTime? TimeMovedToLeadOpportunity { get; set; }
        public DateTime? TimeMovedToLeadClosure { get; set; }
        public bool LeadClosureStatus {get; set;}
        public bool LeadConversionStatus { get; set; } = false;
        public DateTime? TimeConvertedToClient { get; set; }
        public bool IsLeadDropped { get; set; }
        public string DropLearning { get; set; }
        public long? DropReasonId { get; set; }
        public virtual DropReason DropReason { get; set; }
        public long? PrimaryContactId { get; set; }
        public virtual LeadContact PrimaryContact { get; set; }
        public long? SecondaryContactId { get; set; }
        public LeadContact SecondaryContact { get; set; }
        public long? CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public IEnumerable<LeadKeyPerson> LeadKeyPersons { get; set; }
        public IEnumerable<LeadDivision> LeadDivisions { get; set; }
        public long CreatedById { get; set; }
        public virtual UserProfile CreatedBy { get; set; }
        public bool IsDeleted { get; set; } = false;
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }

    }
}
