using Halobiz.Common.DTOs.TransferDTOs;
using HalobizMigrations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class DivisionTransferDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string MissionStatement { get; set; }
        public virtual UserProfileTransferDTO Head { get; set; }
        public IEnumerable<OperatingEntityTransferDTO> OperatingEntities { get; set; }
    }
}
