using HalobizMigrations.Models;
using System;
using System.Collections.Generic;

namespace OnlinePortalBackend.DTOs.TransferDTOs
{
    public class ServiceRatingTransferDTO
    {
        public long Id { get; set; }
        public long ProspectId { get; set; }
        public Prospect Prospect { get; set; }
        public long ServiceId { get; set; }
        public Service Service { get; set; }
        public long Rating { get; set; }
        public string Review { get; set; }
    }

    public class ServiceRatingsDTO
    {
        public double AverageRating { get; set; }
        public IEnumerable<ServiceRatingsDetailDTO> Details { get; set; }
    }

    public class ServiceRatingsDetailDTO
    {
        public long Rating { get; set; }
        public string Review { get; set; }
        public DateTime DatePosted { get; set; }
    }
}