using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HaloBiz.Model.LAMS;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace HaloBiz.Model.ManyToManyRelationship
{
    [Table("LeadDivisionContactLeadEngagement")]
    [Index(nameof(LeadEngagementsId), Name = "IX_LeadDivisionContactLeadEngagement_LeadEngagementsId")]
    public partial class LeadDivisionContactLeadEngagement
    {
        [Key]
        public long ContactsEngagedWithId { get; set; }
        [Key]
        public long LeadEngagementsId { get; set; }

        [ForeignKey(nameof(ContactsEngagedWithId))]
        [InverseProperty(nameof(LeadDivisionContact.LeadDivisionContactLeadEngagements))]
        public virtual LeadDivisionContact ContactsEngagedWith { get; set; }
        [ForeignKey(nameof(LeadEngagementsId))]
        [InverseProperty(nameof(LeadEngagement.LeadDivisionContactLeadEngagements))]
        public virtual LeadEngagement LeadEngagements { get; set; }
    }
}
