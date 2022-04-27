using System;
using System.ComponentModel.DataAnnotations;

namespace OnlinePortalBackend.DTOs.ReceivingDTOs
{
    public class ServiceRatingReceivingDTO
    {
        public long CustomerDivisionId { get; set; }
        public long ServiceId { get; set; }
        public long Rating { get; set; }
        public string Review { get; set; }
    }

    public class AppRatingReceivingDTO
    {
        public long CustomerDivisionId { get; set; }
        public long ApplicationId { get; set; }
        public long Rating { get; set; }
        public string Review { get; set; }
    }
}