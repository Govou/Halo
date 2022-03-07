using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.ProjectManagementDTO
{
    public class UpdateWorkspaceDTO
    {
        public string Caption { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }
        public string Alias { get; set; }
        public string StatusFlowOption { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
