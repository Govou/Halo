using HaloBiz.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class AccountClassTransferDTO
    {
        public long Id { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }
        public long AccountClassAlias { get; set; }
        public IEnumerable<ControlAccountWithoutAccountClassTransferDTO> ControlAccounts { get; set; }
    }

    public class AccountClassWithTotalTransferDTO
    {
        public long Id { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }
        public string AccountClassAlias { get; set; }
        public IEnumerable<ControlAccountWithTotal> ControlAccounts { get; set; }

        public double Total { get; set; }
    }
}
