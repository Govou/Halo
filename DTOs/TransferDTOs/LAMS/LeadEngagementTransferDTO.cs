using System;
using System.Collections.Generic;
using HaloBiz.Model;
using HaloBiz.Model.LAMS;

namespace HaloBiz.DTOs.TransferDTOs.LAMS
{
    public class LeadEngagementTransferDTO
    {
        public long Id { get; set; }
        public string Caption { get; set; }
        public string EngagementDiscussion { get; set; }
        public long EngagementTypeId { get; set; }
        public EngagementType EngagementType { get; set; }
        public DateTime Date { get; set; }
        public long LeadId { get; set; }
        public Lead Lead { get; set; }
        public string LeadCaptureStage { get; set; }
        public string EngagementOutcome { get; set; }
        public string ReasonForEngagement { get; set; }
    }
}