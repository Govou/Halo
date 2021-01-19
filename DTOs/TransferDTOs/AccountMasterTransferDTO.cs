﻿using HaloBiz.Model;
using HaloBiz.Model.AccountsModel;
using HaloBiz.Model.ManyToManyRelationship;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class AccountMasterTransferDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long AccountMasterAlias { get; set; }
        public bool IntegrationFlag { get; set; }
        public long VoucherId { get; set; }
        public long ChartofAccountSubId { get; set; }
        public virtual Account ChartofAccountSub { get; set; }
        public long BranchId { get; set; }
        public virtual Branch Branch { get; set; }
        public long OfficeId { get; set; }
        public virtual Office Office { get; set; }
        public IEnumerable<SBUAccountMaster> SBUAccountMaster { get; set; }
    }
}
