using HalobizMigrations.Models.Complaints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halobiz.Common.DTOs.TransferDTOs
{
    public class ComplaintDTO
    {
        public long ComplaintTypeId { get; set; }
        public long ComplaintOriginId { get; set; }
        public long ComplaintSourceId { get; set; }
        public long ComplainantId { get; set; }
        public string ComplaintDescription { get; set; }
        public DateTime DateCreated { get; set; }
        public List<EvidenceReceivingDTO> Evidences { get; set; }
    }

    public class EvidenceReceivingDTO
    {
        public string Caption { get; set; }
        public string ImageUrl { get; set; }
        public long? ComplaintId { get; set; }
        public ComplaintStage ComplaintStage { get; set; }
    }
}
