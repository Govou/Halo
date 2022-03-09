using HalobizMigrations.Models;
using HalobizMigrations.Models.OnlinePortal;
using System;

namespace OnlinePortalBackend.DTOs.TransferDTOs
{
    public class PortalComplaintTransferDTO
    {
        public long CreatedById { get; set; }
        public string ResolutionComments { get; set; }
        public DateTime? DateResolved { get; set; }
        public bool IsResolved { get; set; }
        public string AdminComments { get; set; }
        public string ComplaintDescription { get; set; }
        public CustomerDivision CustomerDivision { get; set; }
        public long CustomerDivisionId { get; set; }
        public Service Service { get; set; }
        public long ServiceId { get; set; }
        public Prospect Prospect { get; set; }
        public long ProspectId { get; set; }
        public long Id { get; set; }
    }
}