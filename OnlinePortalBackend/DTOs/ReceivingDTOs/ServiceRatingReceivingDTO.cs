using System;
using System.ComponentModel.DataAnnotations;

namespace OnlinePortalBackend.DTOs.ReceivingDTOs
{
    public class ServiceRatingReceivingDTO
    {
        public long ProspectId { get; set; }
        public long ServiceId { get; set; }
        public long Rating { get; set; }
        public string Review { get; set; }
    }
}