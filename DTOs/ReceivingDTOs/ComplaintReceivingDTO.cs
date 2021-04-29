using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class ComplaintReceivingDTO
    {
       /* public long? InvestigationsLedById { get; set; }
        public bool? IsResolved { get; set; }
        public DateTime? DateResolved { get; set; }
        public long? ResolvedById { get; set; }
        public bool? IsConfirmedResolved { get; set; }
        public long? ConfirmedResolvedById { get; set; }
        public bool? IsClosed { get; set; }
        public string ClosedRemarks { get; set; }
        public DateTime? DateClosed { get; set; }
        public long? ClosedById { get; set; }
        public DateTime? DateInvestigationsCompleted { get; set; }
        public bool? IsInvestigated { get; set; }
        public long? AssesedLedById { get; set; }*/
        public long ComplaintTypeId { get; set; }
        public long ComplaintOriginId { get; set; }
        public long ComplaintSourceId { get; set; }
        public long ComplainantId { get; set; }
        public string ComplaintDescription { get; set; }
        /*public DateTime DateCreated { get; set; }
        public DateTime DateRegistered { get; set; }
        public bool? IsRegistered { get; set; }
        public long RegisteredById { get; set; }
        public string TrackingId { get; set; }
        public bool? IsAssesed { get; set; }
        public DateTime? DateAssesmentsConcluded { get; set; }*/
    }
}
