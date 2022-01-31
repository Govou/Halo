using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HaloBiz.Model.LAMS;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace HaloBiz.Model.ManyToManyRelationship
{
    [Table("LeadEngagementUserProfile")]
    [Index(nameof(UsersEngagedWithId), Name = "IX_LeadEngagementUserProfile_UsersEngagedWithId")]
    public partial class LeadEngagementUserProfile
    {
        [Key]
        public long UserLeadEngagementsId { get; set; }
        [Key]
        public long UsersEngagedWithId { get; set; }

        [ForeignKey(nameof(UserLeadEngagementsId))]
        [InverseProperty(nameof(LeadEngagement.LeadEngagementUserProfiles))]
        public virtual LeadEngagement UserLeadEngagements { get; set; }
        [ForeignKey(nameof(UsersEngagedWithId))]
        [InverseProperty(nameof(UserProfile.LeadEngagementUserProfiles))]
        public virtual UserProfile UsersEngagedWith { get; set; }
    }
}
