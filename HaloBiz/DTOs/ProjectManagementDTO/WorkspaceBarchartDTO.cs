using System;
using System.Collections.Generic;

namespace HaloBiz.DTOs.ProjectManagementDTO
{
    public class WorkspaceBarchartDTO
    {
        public List<string> WorkspaceName { get; set; }

        public List<long> TaskCount { get; set; }
    }
}
