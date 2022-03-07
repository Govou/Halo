using HaloBiz.Model.ManyToManyRelationship;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaloBiz.Model.LAMS
{
    [Index(nameof(ClientContactQualificationId), Name = "IX_LeadDivisionKeyPeople_ClientContactQualificationId")]
    [Index(nameof(CreatedById), Name = "IX_LeadDivisionKeyPeople_CreatedById")]
    [Index(nameof(CustomerDivisionId), Name = "IX_LeadDivisionKeyPeople_CustomerDivisionId")]
    [Index(nameof(DesignationId), Name = "IX_LeadDivisionKeyPeople_DesignationId")]
    [Index(nameof(LeadDivisionId), Name = "IX_LeadDivisionKeyPeople_LeadDivisionId")]
    public class LeadDivisionKeyPerson : Contact
    {
        public LeadDivisionKeyPerson()
        {
            LeadDivisionKeyPersonLeadEngagements = new HashSet<LeadDivisionKeyPersonLeadEngagement>();
        }

        public long LeadDivisionId { get; set; }
        public virtual LeadDivision LeadDivision { get; set; }
        public long? CustomerDivisionId { get; set; }
        public CustomerDivision CustomerDivision { get; set; }
        [InverseProperty(nameof(LeadDivisionKeyPersonLeadEngagement.KeyPersonsEngagedWith))]
        public virtual ICollection<LeadDivisionKeyPersonLeadEngagement> LeadDivisionKeyPersonLeadEngagements { get; set; }
    }
}