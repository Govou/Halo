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
    [Index(nameof(ClientContactQualificationId), Name = "IX_LeadDivisionContacts_ClientContactQualificationId")]
    [Index(nameof(CreatedById), Name = "IX_LeadDivisionContacts_CreatedById")]
    [Index(nameof(DesignationId), Name = "IX_LeadDivisionContacts_DesignationId")]
    public class LeadDivisionContact : Contact
    {
        [Required]
        public ContactType Type { get; set; }
        [InverseProperty(nameof(LeadDivisionContactLeadEngagement.ContactsEngagedWith))]
        public virtual ICollection<LeadDivisionContactLeadEngagement> LeadDivisionContactLeadEngagements { get; set; }
    }
}
