using System;
using System.Collections.Generic;

namespace HaloBiz.DTOs.ProjectManagementDTO
{
    public class WorkspaceRoot
    {
        public long Id { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }
        public string Alias { get; set; }
        public bool IsActive { get; set; }
        public string StatusFlowOption { get; set; }
        public List<StatusCategoryDTO> StatusCategoryDTO { get; set; }

    }
}
