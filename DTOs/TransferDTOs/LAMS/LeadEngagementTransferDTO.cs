using System;
using System.Collections.Generic;
using HaloBiz.Model;
using HaloBiz.Model.LAMS;
using HaloBiz.Model.ManyToManyRelationship;

namespace HaloBiz.DTOs.TransferDTOs.LAMS
{
    public class LeadEngagementTransferDTO
    {
        public long Id { get; set; }
        public string Caption { get; set; }
        public string EngagementDiscussion { get; set; }
        public long EngagementTypeId { get; set; }
        public EngagementType EngagementType { get; set; }
        public long EngagementReasonId { get; set; }
        public EngagementReason EngagementReason { get; set; }
        public DateTime Date { get; set; }
        public long LeadId { get; set; }
        public Lead Lead { get; set; }
        public string LeadCaptureStage { get; set; }
        public string EngagementOutcome { get; set; }
        public string DocumentsUrl { get; set; }
        public ICollection<LeadDivisionContactLeadEngagementTransferDTO> LeadDivisionContactLeadEngagements { get; set; }
        public ICollection<LeadDivisionKeyPersonLeadEngagementTransferDTO> LeadDivisionKeyPersonLeadEngagements { get; set; }
        public ICollection<LeadEngagementUserProfileTransferDTO> LeadEngagementUserProfiles { get; set; }
    }
}