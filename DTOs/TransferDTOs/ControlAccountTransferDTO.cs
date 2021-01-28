using System.Collections.Generic;
using HaloBiz.Model.AccountsModel;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class ControlAccountTransferDTO
    {
        public long Id { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }
        public long Alias { get; set; }
        public AccountClass AccountClass { get; set; }
        public IEnumerable<Account> Accounts { get; set; }
    }
}