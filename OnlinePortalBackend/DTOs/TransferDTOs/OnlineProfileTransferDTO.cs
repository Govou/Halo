using HalobizMigrations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePortalBackend.DTOs.TransferDTOs
{

    public class OnlineProfileTransferDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool LockoutEnabled { get; set; } = false;
        public int AccessFailedCount { get; set; } = 0;
        public long CustomerDivisionId { get; set; }
        public CustomerDivision CustomerDivision { get; set; }
    }
}
