using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Model.LAMS
{
    public class LeadEngagement
    {
        [Key]
        public long Id { get; set; }
        public string Caption { get; set; }
        public string EngagementDiscussion { get; set; }
        [Required]
        public long EngagementTypeId { get; set; }
        public EngagementType EngagementType { get; set; }
        public long? LeadKeyContactId { get; set; }
        public LeadContact LeadKeyContact { get; set; }
        public long? LeadKeyPersonId { get; set; }
        public LeadKeyPerson LeadKeyPerson { get; set; }
        public DateTime Date { get; set; }
        public long LeadId { get; set; }
        public Lead Lead { get; set; }
        public string LeadCaptureStage { get; set; }
        public string EngagementOutcome { get; set; }
        [Required]
        public long CreatedById { get; set; }
        public UserProfile CreatedBy { get; set; }
        public bool IsDeleted { get; set; } = false;
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }
    }
}
