using HalobizMigrations.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halobiz.Common.DTOs.TransferDTOs
{
    public class OnlineProfileTransferDTO
    {
            public string Name { get; set; }
            public string Email { get; set; }
            public bool EmailConfirmed { get; set; } = false;
            public bool LockoutEnabled { get; set; } = false;
            public int AccessFailedCount { get; set; } = 0;
            public long CustomerDivisionId { get; set; }
            public Origin Origin { get; set; }        
    }
}
