using HalobizMigrations.Models;
using HalobizMigrations.Models.Armada;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class PilotTransferDTO
    {
    }

    public class PilotTypeTransferDTO
    {
        public long Id { get; set; }
        public string TypeName { get; set; }
        public string TypeDesc { get; set; }
        public long CreatedById { get; set; }
        //public UserProfile CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class PilotRankTransferDTO
    {
        public long Id { get; set; }
        public string RankName { get; set; }
        public string Alias { get; set; }
        public string Description { get; set; }
        public long Sequence { get; set; }
        public long PilotTypeId { get; set; }
        public long CreatedById { get; set; }
      
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }



        public PilotType PilotType { get; set; }
      
        public UserProfile CreatedBy { get; set; }
    }
}
