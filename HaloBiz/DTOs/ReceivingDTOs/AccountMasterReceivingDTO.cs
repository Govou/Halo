using HalobizMigrations.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class AccountMasterReceivingDTO
    {
        [StringLength(1000)]
        public string Description { get; set; }
        [Required]
        public bool IntegrationFlag { get; set; }
        public string TransactionId { get; set; }
        public string DTrackJournalCode { get; set; }

        [Required]
        public double Value { get; set; }
        [Required]
        public long VoucherId { get; set; }
        [Required]
        public long BranchId { get; set; }
        [Required]
        public long OfficeId { get; set; }
        public long CustomerDivisionId { get; set; }
    }
}
