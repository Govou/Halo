using System;

namespace OnlinePortalBackend.DTOs.TransferDTOs
{
    public class EndorsementTrackingDTO
    {
        public int EndorsementProcessingCount { get; set; }
        public int EndorsementHistoryCount { get; set; }
        public string ServiceName { get; set; }
        public DateTime EndorsementRequestDate { get; set; }
        public string RequestExecution { get; set; }
    }
}
