using System;
using System.ComponentModel.DataAnnotations;

namespace OnlinePortalBackend.DTOs.ReceivingDTOs
{
    public class PortalComplaintReceivingDTO
    {
        public string ResolutionComments { get; set; }
        public DateTime? DateResolved { get; set; }
        public bool IsResolved { get; set; }
        public string AdminComments { get; set; }
        public string ComplaintDescription { get; set; }
        public long CustomerDivisionId { get; set; }
        public long ServiceId { get; set; }
        public long ProspectId { get; set; }
    }
}