using System.Collections.Generic;
using HalobizMigrations.Models;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class ZoneTransferDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long HeadId { get; set; }
        public virtual UserProfile Head { get; set; }
        public long StateId { get; set; }
        public State State { get; set; }
        public long LGAId { get; set; }
        public Lga LGA { get; set; }
        public long RegionId { get; set; }
        public Region Region { get; set; }

    }
}