using System.Collections.Generic;
using HaloBiz.Model;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class RegionTransferDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long BranchId { get; set; }
        public Branch Branch { get; set; }
        public long HeadId { get; set; }
        public virtual UserProfile Head { get; set; }
        public IEnumerable<Zone> Zones { get; set; }

    }
}