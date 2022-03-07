using HalobizMigrations.Models.Halobiz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class ActivityReceivingDTO
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Description { get; set; }
        public ActivityType ActivityType { get; set; }
        public ActivityStatus ActivityStatus { get; set; }
    }
}
