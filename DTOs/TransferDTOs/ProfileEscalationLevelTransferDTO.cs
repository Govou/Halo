using HalobizMigrations.Models;
using HalobizMigrations.Models.Complaints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class ProfileEscalationLevelTransferDTO
    {
        public long Id { get; set; }
        public long EscalationLevelId { get; set; }
        public long UserProfileId { get; set; }
        public EscalationLevel EscalationLevel { get; set; }
        public UserProfile UserProfile { get; set; }
    }
}
