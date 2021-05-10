﻿using HalobizMigrations.Models.Complaints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class ComplaintDTO
    {
    }
    public class ComplaintHandlingDTO
    {
        public List<ComplaintTransferDTO> assignedComplaints { get; set; }
        public List<ComplaintTransferDTO> workbenchComplaints { get; set; }
        public int TotalComplaintsAssigned { get; set; }
        public int TotalComplaintsInWorkbench { get; set; }
        public int TotalComplaintsBeingHandled { get; set; }
        public int TotalComplaintsClosed { get; set; }
    }

    public class ComplaintHandlingStatsDTO
    {
        public int TotalComplaintsAssigned { get; set; }
        public int TotalComplaintsInWorkbench { get; set; }
        public int TotalComplaintsBeingHandled { get; set; }
        public int TotalComplaintsClosed { get; set; }
    }

    public class ComplaintTrackingTransferDTO
    {
        public ComplaintTransferDTO Complaint { get; set; }
        public ComplaintAssessmentTransferDTO Assessment { get; set; }
        public ComplaintInvestigationTransferDTO Investigation { get; set; }
        public ComplaintResolutionTransferDTO Resolution { get; set; }
        public List<string> RegistrationEvidenceUrls { get; set; }
        public List<string> AssessmentEvidenceUrls { get; set; }
        public List<string> InvestigationEvidenceUrls { get; set; }
        public List<string> ResolutionEvidenceUrls { get; set; }
        public decimal TotalHandlerCasesResolved { get; set; }
        public decimal TotalHanlderCasesUnresolved { get; set; }
        public int TotalHandlerCases { get; set; }
        public string UserProfileImageUrl { get; set; }
    }

    public class ComplaintAssessmentTransferDTO
    {
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public long CreatedById { get; set; }
        public DateTime? ConclusionDate { get; set; }
        public DateTime CommencementDate { get; set; }
        public DateTime CapturedDateTime { get; set; }
        public string AssesmentDetails { get; set; }
        public string Findings { get; set; }
        public long CapturedById { get; set; }
        public long ComplaintId { get; set; }
    }

    public class ComplaintInvestigationTransferDTO
    {
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public long CreatedById { get; set; }
        public DateTime? ConclusionDate { get; set; }
        public DateTime CommencementDate { get; set; }
        public DateTime CapturedDateTime { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Findings { get; set; }
        public string InvestigationDetails { get; set; }
        public long CapturedById { get; set; }
        public long ComplaintId { get; set; }
    }

    public class ComplaintResolutionTransferDTO
    {
        public string RootCause { get; set; }
        public string ResolutionDetails { get; set; }
        public string Learnings { get; set; }
        public long ComplaintId { get; set; }
        public long CapturedById { get; set; }
        public DateTime CapturedDateTime { get; set; }
        public long CreatedById { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
