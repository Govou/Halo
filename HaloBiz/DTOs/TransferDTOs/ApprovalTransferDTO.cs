using HalobizMigrations.Models;

using System;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class ApprovalTransferDTO
    {
        public long Id { get; set; }

        public string Caption { get; set; }

        public DateTime DateTimeApproved { get; set; }

        public bool IsApproved { get; set; }
        public long ResponsibleId { get; set; }
        public UserProfile Responsible { get; set; }

        public long Sequence { get; set; }
        public string Level { get; set; }
        public long? QuoteServiceId { get; set; }
        public QuoteService QuoteService { get; set; }

        public long? QuoteId { get; set; }
        public Quote Quote { get; set; }

        public long? ContractId { get; set; }
        public Contract Contract { get; set; }

        public long? ContractServiceId { get; set; }
        public ContractService ContractService { get; set; }

        public long? ServiceId { get; set; }
        public Service Service { get; set; }

        public long CreatedById { get; set; }
        public UserProfile CreatedBy { get; set; }

    }

    public class ApprovingLevelBackUp
    {
        public long CurrentUserId { get; set; }
        public System.Collections.Generic.List<ApprovingLevelBackUpInfo> BackUpInfos { get; set; }

    }

    public class ApprovingLevelBackUpInfo
    {
        public string Level { get; set; }
        public bool CanApprove { get; set; }
    }
}