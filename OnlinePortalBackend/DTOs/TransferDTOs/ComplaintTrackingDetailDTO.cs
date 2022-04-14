using System;
using System.Collections.Generic;

namespace OnlinePortalBackend.DTOs.TransferDTOs
{
    public class ComplaintTrackingDetailDTO
    {
        public string TrackingId { get; set; }
        public DateTime RegisteredDate { get; set; }
        public ComplaintRegistrationTracking ComplaintRegistration { get; set; }
        public ComplaintAssessmentTracking ComplaintAssessment { get; set; }
        public ComplaintInvestigationTracking ComplaintInvestigation { get; set; }
        public ComplaintResolutionTracking ComplaintResolution { get; set; }
        public ComplaintClosedTracking ComplaintClosed { get; set; }
    }

    public class ComplaintRegistrationTracking
    {
        public List<string> RegistrationEvidenceUrls { get; set; }
    }

    public class ComplaintAssessmentTracking
    {
        public DateTime? CapturedDate { get; set; }
        public string AssesmentDetails { get; set; }
        public string Findings { get; set; }
        public List<string> AssessmentEvidenceUrls { get; set; }
    }

    public class ComplaintInvestigationTracking
    {
        public DateTime? CapturedDate { get; set; }
        public DateTime? ConcludedDate { get; set; }
        public string InvestigationDetails { get; set; }
        public string Findings { get; set; }
        public List<string> InvestigationEvidenceUrls { get; set; }
    }

    public class ComplaintResolutionTracking
    {
        public DateTime? CapturedDate { get; set; }
        public string RootCause { get; set; }
        public string ResolutionDetails { get; set; }
        public string Learnings { get; set; }
        public List<string> ResolutionEvidenceUrls { get; set; }
    }

    public class ComplaintClosedTracking
    {
        public List<string> ClosureEvidenceUrls { get; set; }
    }
}







