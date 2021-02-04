using HaloBiz.Model;
using HaloBiz.Model.AccountsModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class AccountTransferDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long Alias { get; set; }
        public IEnumerable<AccountMaster> AccountMasters { get; set; }
        public bool IsDebitBalance { get; set; }
        public long ControlAccountId { get; set; }
        public virtual ControlAccount ControlAccount { get; set; }
    }
}
