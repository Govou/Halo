using HaloBiz.Model;
using HaloBiz.Model.AccountsModel;
using HaloBiz.Model.ManyToManyRelationship;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class AccountMasterReceivingDTO
    {
        [Required]
        public string Name { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        [Required]
        public long AccountMasterAlias { get; set; }
        [Required]
        public bool IntegrationFlag { get; set; }
        public string TransactionId { get; set; }

        [Required]
        public long VoucherId { get; set; }
        [Required]
        public long AccountId { get; set; }
        [Required]
        public long BranchId { get; set; }
        [Required]
        public long OfficeId { get; set; }
    }
}
