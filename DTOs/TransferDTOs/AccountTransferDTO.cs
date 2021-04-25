using HalobizMigrations.Models;
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
        public string Alias { get; set; }
        public bool IsDebitBalance { get; set; }
        public bool IsActive { get; set; }
        public long ControlAccountId { get; set; }
        public IEnumerable<AccountDetailWithoutAccountMasterTransferDTO> AccountDetails { get; set; }
    }
}
