using System;
using System.ComponentModel.DataAnnotations;

namespace OnlinePortalBackend.DTOs.ReceivingDTOs
{
    public class ServiceRatingReceivingDTO
    {
        public long CustomerDivisionId { get; set; }
        public long ServiceId { get; set; }
        public long Rating { get; set; }
        public int Recommendation { get; set; }
        public string Review { get; set; }
    }

    public class AppRatingReceivingDTO
    {
        public long CustomerDivisionId { get; set; }
        public long ApplicationId { get; set; }
        public long Rating { get; set; }
        public string Review { get; set; }
    }

    public class AppRatingTransferDTO
    {
        public long CustomerDivisionId { get; set; }
        public string CustomerName { get; set; }
        public string ApplicationName { get; set; }
        public DateTime DateRated { get; set; }
        public long ApplicationId { get; set; }
        public long Rating { get; set; }
        public string Review { get; set; }
    }
}