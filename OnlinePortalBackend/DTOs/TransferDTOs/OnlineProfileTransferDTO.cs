﻿using HalobizMigrations.Models;
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

    public class OnlineProfileTransferDetailDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool LockoutEnabled { get; set; } = false;
        public int AccessFailedCount { get; set; } = 0;
        public long? CustomerDivisionId { get; set; }
        public List<ProfileContractDetail> ProfileContractDetail { get; set; }

    }

    public class ProfileContractDetail
    {
        public long? ContractId { get; set; }
        public long? ScheduledContractId { get; set; }
        public List<ProfileContractServiceDetail> ContractServices { get; set; }

    }

    public class ProfileContractServiceDetail
    {
        public long ContractServiceId { get; set; }
        public long ServiceId { get; set; }
    }
}
