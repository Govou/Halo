using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class AccountDetailReceivingDTO
    {
        [Required]
        public string Name { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        [Required]
        public long AccountDetailsAlias { get; set; }
        [Required]
        public bool IntegrationFlag { get; set; }
        [Required]
        public long VoucherId { get; set; }
        [Required]
        public string TransactionId { get; set; }
        [Required]
        public DateTime TransactionDate { get; set; }
        [Required]
        public double Credit { get; set; }
        [Required]
        public double Debit { get; set; }
        [Required]
        public long BranchId { get; set; }
        [Required]
        public long OfficeId { get; set; }
        [Required]
        public long AccountMasterId { get; set; }
        public long AccountClassAlias { get; set; }
    }
}
