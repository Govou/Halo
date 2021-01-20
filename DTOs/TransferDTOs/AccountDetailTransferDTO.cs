﻿using HaloBiz.Model;
using HaloBiz.Model.AccountsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class AccountDetailTransferDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long AccountDetailsAlias { get; set; }
        public bool IntegrationFlag { get; set; }
        public long VoucherId { get; set; }
        public long TransactionId { get; set; }
        public DateTime TransactionDate { get; set; }
        public double Credit { get; set; }
        public double Debit { get; set; }
        public long BranchId { get; set; }
        public virtual Branch Branch { get; set; }
        public long OfficeId { get; set; }
        public virtual Office Office { get; set; }
        public long AccountMasterId { get; set; }
        public virtual AccountMaster AccountMaster { get; set; }
        public long AccountClassAlias { get; set; }
    }
}
