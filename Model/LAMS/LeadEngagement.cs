using HaloBiz.Model.ManyToManyRelationship;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Model.LAMS
{
    [Index(nameof(CreatedById), Name = "IX_LeadEngagements_CreatedById")]
    [Index(nameof(EngagementTypeId), Name = "IX_LeadEngagements_EngagementTypeId")]
    [Index(nameof(LeadId), Name = "IX_LeadEngagements_LeadId")]
    public class LeadEngagement
    {
        public LeadEngagement()
        {
            LeadDivisionContactLeadEngagements = new HashSet<LeadDivisionContactLeadEngagement>();
            LeadDivisionKeyPersonLeadEngagements = new HashSet<LeadDivisionKeyPersonLeadEngagement>();
            LeadEngagementUserProfiles = new HashSet<LeadEngagementUserProfile>();
        }

        [Key]
        public long Id { get; set; }
        public string Caption { get; set; }
        public string EngagementDiscussion { get; set; }
        [Required]
        public long EngagementTypeId { get; set; }
        public EngagementType EngagementType { get; set; }
        [Required]
        public long EngagementReasonId { get; set; }
        public EngagementReason EngagementReason { get; set; }
        public DateTime Date { get; set; }
        public long LeadId { get; set; }
        public Lead Lead { get; set; }
        public string LeadCaptureStage { get; set; }
        public string EngagementOutcome { get; set; }
        public string DocumentsUrl { get; set; }
        [Required]
        public long CreatedById { get; set; }
        public UserProfile CreatedBy { get; set; }
        public bool IsDeleted { get; set; } = false;
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }

        [InverseProperty(nameof(LeadDivisionContactLeadEngagement.LeadEngagements))]
        public virtual ICollection<LeadDivisionContactLeadEngagement> LeadDivisionContactLeadEngagements { get; set; }

        [InverseProperty(nameof(LeadDivisionKeyPersonLeadEngagement.LeadEngagements))]
        public virtual ICollection<LeadDivisionKeyPersonLeadEngagement> LeadDivisionKeyPersonLeadEngagements { get; set; }

        [InverseProperty(nameof(LeadEngagementUserProfile.UserLeadEngagements))]
        public virtual ICollection<LeadEngagementUserProfile> LeadEngagementUserProfiles { get; set; }
    }
}
