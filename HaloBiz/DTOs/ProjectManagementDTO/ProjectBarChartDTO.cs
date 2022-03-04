using System;
using System.Collections.Generic;

namespace HaloBiz.DTOs.ProjectManagementDTO
{
    public class ProjectBarChartDTO
    {
        public List<string> ProjectNames { get; set; }

        public List<long> TaskCount { get; set; }
    }
}
