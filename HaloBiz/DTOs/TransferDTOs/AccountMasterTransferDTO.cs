using HalobizMigrations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class AccountMasterTransferDTO
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public string AccountMasterAlias { get; set; }
        public bool IntegrationFlag { get; set; }
        public long VoucherId { get; set; }
        public FinanceVoucherType Voucher { get; set; }
        public string TransactionId { get; set; }
        public string DTrackJournalCode { get; set; }
        public double Value { get; set; }
        public long BranchId { get; set; }
        public virtual Branch Branch { get; set; }
        public long OfficeId { get; set; }
        public virtual Office Office { get; set; }
        public long CustomerDivisionId { get; set; }
        public CustomerDivisionWithoutObjectsTransferDTO CustomerDivision { get; set; }

        public IEnumerable<SbuaccountMaster> SbuaccountMaster { get; set; }
        public IEnumerable<AccountDetailWithoutAccountMasterTransferDTO> AccountDetails { get; set; }
    }
}
