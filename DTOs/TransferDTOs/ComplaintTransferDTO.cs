using HalobizMigrations.Models;
using HalobizMigrations.Models.Complaints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class ComplaintTransferDTO
    {
        public long? InvestigationsLedById { get; set; }
        public UserProfile InvestigationsLedBy { get; set; }
        public bool? IsResolved { get; set; }
        public DateTime? DateResolved { get; set; }
        public long? ResolvedById { get; set; }
        public UserProfile ResolvedBy { get; set; }
        public bool? IsConfirmedResolved { get; set; }
        public long? ConfirmedResolvedById { get; set; }
        public UserProfile ConfirmedResolvedBy { get; set; }
        public bool? IsClosed { get; set; }
        public string ClosedRemarks { get; set; }
        public DateTime? DateClosed { get; set; }
        public long? ClosedById { get; set; }
        public UserProfile ClosedBy { get; set; }
        public DateTime? DateInvestigationsCompleted { get; set; }
        public long CreatedById { get; set; }
        public bool? IsInvestigated { get; set; }
        public long? AssesedLedById { get; set; }
        public long Id { get; set; }
        public long ComplaintTypeId { get; set; }
        public virtual ComplaintType ComplaintType { get; set; }
        public long ComplaintOriginId { get; set; }
        public virtual ComplaintOrigin ComplaintOrigin { get; set; }
        public long ComplaintSourceId { get; set; }
        public virtual ComplaintSource ComplaintSource { get; set; }
        public long ComplainantId { get; set; }
        public string ComplaintDescription { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateRegistered { get; set; }
        public bool? IsRegistered { get; set; }
        public long RegisteredById { get; set; }
        public UserProfile RegisteredBy { get; set; }
        public string TrackingId { get; set; }
        public bool? IsAssesed { get; set; }
        public DateTime? DateAssesmentsConcluded { get; set; }
        public UserProfile AssesedLedBy { get; set; }
    }
}
