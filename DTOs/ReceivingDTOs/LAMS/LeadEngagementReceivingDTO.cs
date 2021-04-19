using System;
using System.ComponentModel.DataAnnotations;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class LeadEngagementReceivingDTO
    {
        public string Caption { get; set; }
        public string EngagementDiscussion { get; set; }
        public long EngagementTypeId { get; set; }
        public DateTime Date { get; set; }
        public long LeadId { get; set; }
        public string LeadCaptureStage { get; set; }
        public string EngagementOutcome { get; set; }
        public string ReasonForEngagement { get; set; }
    }
}