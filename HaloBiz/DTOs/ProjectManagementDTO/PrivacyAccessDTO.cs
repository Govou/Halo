using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.ProjectManagementDTO
{
    public class PrivacyAccessDTO
    {
        public bool IsActive { get; set; }
        public long? WorkspaceId { get; set; }
        public long? PrivacyAccessId { get; set; }
        public long CreatedById { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
