using HalobizMigrations.Models;
using HalobizMigrations.Models.Armada;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class CommanderTypeAndRankTransferDTO
    {
        public long Id { get; set; }
        public string TypeName { get; set; }
      
        public string TypeDesc { get; set; }
        public bool isDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        //public string TypeDesc { get; set; }
    }

    public class CommanderRankTransferDTO
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public string Alias { get; set; }
        public string RankName { get; set; }
        public CommanderType CommanderType { get; set; }
        public long CommanderTypeId { get; set; }
        public long Sequence { get; set; }
        public bool isDeleted { get; set; }
        public DateTime CreatedAt { get; set; }

        public  CommanderRank PreceedingRank { get; set; }
        public  CommanderRank NextRank { get; set; }
        public  UserProfile CreatedBy { get; set; }
    }
}
