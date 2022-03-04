using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HaloBiz.Model.LAMS;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace HaloBiz.Model.ManyToManyRelationship
{
    [Table("LeadDivisionKeyPersonLeadEngagement")]
    [Index(nameof(LeadEngagementsId), Name = "IX_LeadDivisionKeyPersonLeadEngagement_LeadEngagementsId")]
    public partial class LeadDivisionKeyPersonLeadEngagement
    {
        [Key]
        public long KeyPersonsEngagedWithId { get; set; }
        [Key]
        public long LeadEngagementsId { get; set; }

        [ForeignKey(nameof(KeyPersonsEngagedWithId))]
        [InverseProperty(nameof(LeadDivisionKeyPerson.LeadDivisionKeyPersonLeadEngagements))]
        public virtual LeadDivisionKeyPerson KeyPersonsEngagedWith { get; set; }
        [ForeignKey(nameof(LeadEngagementsId))]
        [InverseProperty(nameof(LeadEngagement.LeadDivisionKeyPersonLeadEngagements))]
        public virtual LeadEngagement LeadEngagements { get; set; }
    }
}
