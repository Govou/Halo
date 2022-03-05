using HalobizMigrations.Models;
using HalobizMigrations.Models.Complaints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class EvidenceTransferDTO
    {
        public long Id { get; set; }
        public string Caption { get; set; }
        public string ImageUrl { get; set; }
        public long ComplaintId { get; set; }
        public Complaint Complaint { get; set; }
        public ComplaintStage ComplaintStage { get; set; }
        public DateTime DateCaptured { get; set; }
        public long EvidenceCaptureById { get; set; }
        public UserProfile EvidenceCaptureBy { get; set; }
    }
}
