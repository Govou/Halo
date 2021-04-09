using System;
using System.ComponentModel.DataAnnotations;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class ClientEngagementReceivingDTO
    {
        public string Caption { get; set; }
        public string EngagementDiscussion { get; set; }
        public long EngagementTypeId { get; set; }
        public long? LeadKeyContactId { get; set; }
        public long? LeadKeyPersonId { get; set; }
        public DateTime Date { get; set; }
        public long CustomerDivisionId { get; set; }
        public string EngagementOutcome { get; set; }
        public string ContractServicesDiscussed { get; set; }
    }
}