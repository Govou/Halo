﻿using HalobizMigrations.Models;
using HalobizMigrations.Models.Armada;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class CommanderProfileTransferDTO
    {
        public DateTime CreatedAt { get; set; }
        public long CreatedById { get; set; }
        public long CommanderTypeId { get; set; }
        public long AttachedOfficeId { get; set; }
        public long AttachedBranchId { get; set; }
        public long RankId { get; set; }
        public long ProfileId { get; set; }
        public long Id { get; set; }
        public bool IsDeleted { get; set; }

     
        public CommanderType CommanderType { get; set; }
      
        public Office AttachedOffice { get; set; }
       
        public CommanderRank Rank { get; set; }
       
        public UserProfile Profile { get; set; }
       
        public Branch AttachedBranch { get; set; }
    }
}
