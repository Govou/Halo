using HalobizMigrations.Models;
using System;
using System.Collections.Generic;

namespace OnlinePortalBackend.DTOs.TransferDTOs
{
    public class EndorsementDTO
    {
        public int Quantity { get; set; }
        public DateTime DateOfEffect { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Description { get; set; }
        public string DocumentUrl { get; set; }
        public int EndorsementType { get; set; }
        public int EndorsementTypeId { get; set; }
        public int ContractServiceId { get; set; }
        public long Id { get; set; }
        public EndorsementTrackingDTO endorsementTracking { get; set; }
    }

    public class EndorsementList
    {
        public int EndorsementProcessingCount { get; set; }
        public int EndorsementHistoryCount { get; set; }
        public IEnumerable<EndorsementDTO> EndorsementDTOs { get; set; }
    }
}
