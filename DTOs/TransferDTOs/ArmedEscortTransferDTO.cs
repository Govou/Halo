using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class ArmedEscortTransferDTO
    {
    }

    public class ArmedEscortTypeTransferDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }
        public long CreatedById { get; set; }
    }

    public class ArmedEscortRankTransferDTO
    {
        public long Id { get; set; }
        public string RankName { get; set; }
        public string Alias { get; set; }
        public string Description { get; set; }
        public long Sequence { get; set; }
        public long CreatedById { get; set; }
        public long ArmedEscortTypeId { get; set; }
        public DateTime CreatedAt { get; set; }
        
        //public virtual ArmedEscortType ArmedEscortType { get; set; }
    }
}
