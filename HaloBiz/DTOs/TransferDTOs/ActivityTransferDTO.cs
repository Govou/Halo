using HalobizMigrations.Models.Halobiz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class ActivityTransferDTO
    {
        public long Id { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Description { get; set; }
        public ActivityType ActivityType { get; set; }
        public ActivityStatus ActivityStatus { get; set; }
        public long CreatedById { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
