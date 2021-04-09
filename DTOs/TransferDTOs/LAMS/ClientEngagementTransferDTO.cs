using System;
using System.Collections.Generic;
using HaloBiz.Model;
using HaloBiz.Model.LAMS;

namespace HaloBiz.DTOs.TransferDTOs.LAMS
{
    public class ClientEngagementTransferDTO
    {
        public long Id { get; set; }
        public string Caption { get; set; }
        public string EngagementDiscussion { get; set; }
        public long EngagementTypeId { get; set; }
        public EngagementType EngagementType { get; set; }
        public long? LeadKeyContactId { get; set; }
        public LeadContact LeadKeyContact { get; set; }
        public long? LeadKeyPersonId { get; set; }
        public LeadKeyPerson LeadKeyPerson { get; set; }
        public DateTime Date { get; set; }
        public long CustomerDivisionId { get; set; }
        public CustomerDivision CustomerDivision { get; set; }
        public string EngagementOutcome { get; set; }
        public string ContractServicesDiscussed { get; set; }
    }
}