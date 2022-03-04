using System;
using System.Collections.Generic;

namespace HaloBiz.DTOs.ProjectManagementDTO
{
    public class WorkLoadDTO
    {
        public List<DeliverableUser> DeliverableUser { get; set; }
        public List<long> assignedRate { get; set; }
        public List<long> pickedRate { get; set; }
    }
}
