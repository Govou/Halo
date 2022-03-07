using HalobizMigrations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class BranchTransferDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public int HeadId { get; set; }
        public UserProfileTransferDTO Head { get; set; }
        public IEnumerable<OfficeTransferDTO> Offices { get; set; }
    }
}
