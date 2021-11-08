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
        public string CommanderType { get; set; }
        public bool isDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
