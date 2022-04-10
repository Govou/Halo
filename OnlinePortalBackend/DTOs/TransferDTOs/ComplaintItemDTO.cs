using System;

namespace OnlinePortalBackend.DTOs.TransferDTOs
{
    public class ComplaintItemDTO
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string Description { get; set; }
        public string TrackingId { get; set; }
    }
}
