using System;
using System.Collections.Generic;
using HalobizMigrations.Models;

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
        public ICollection<UserProfileTransferDTO> UsersEngagedWith { get; set; }
        public ICollection<LeadDivisionContactTransferDTO> ContactsEngagedWith { get; set; }
        public ICollection<LeadDivisionKeyPersonTransferDTO> KeyPersonsEngagedWith { get; set; }
    }
}