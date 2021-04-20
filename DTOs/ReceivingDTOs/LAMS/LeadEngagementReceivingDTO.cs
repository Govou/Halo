using System;
using System.ComponentModel.DataAnnotations;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class LeadEngagementReceivingDTO
    {
        public string Caption { get; set; }
        public string EngagementDiscussion { get; set; }
        public long EngagementTypeId { get; set; }
        public long EngagementReasonId { get; set; }
        public DateTime Date { get; set; }
        public long LeadId { get; set; }
        public string LeadCaptureStage { get; set; }
        public string EngagementOutcome { get; set; }
        public string DocumentsUrl { get; set; }
        public long[] ContactsEngagedIds { get; set; }
        public long[] KeyPersonsEngagedIds { get; set; }
        public long[] UsersEngagedIds { get; set; }
    }
}