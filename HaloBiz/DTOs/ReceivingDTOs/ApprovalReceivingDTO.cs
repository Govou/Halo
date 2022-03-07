using HalobizMigrations.Models;
using System;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class ApprovalReceivingDTO
    {

        public string Caption { get; set; }

        public DateTime DateTimeApproved { get; set; }

        public bool IsApproved { get; set; }
        public long ResponsibleId { get; set; }

        public long Sequence { get; set; }

        public long? QuoteServiceId { get; set; }

        public long? QuoteId { get; set; }

        public long? ContractId { get; set; }

        public long? ContractServiceId { get; set; }

        public long? ServiceId { get; set; }

        public long CreatedById { get; set; }
        public UserProfile CreatedBy { get; set; }
    }
}