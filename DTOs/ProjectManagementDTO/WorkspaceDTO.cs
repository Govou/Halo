using HalobizMigrations.Models.ProjectManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.ProjectManagementDTO
{
    public class WorkspaceDTO
    {
        public long Id { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }
        public string Alias { get; set; }
        public bool IsActive { get; set; }
        public string StatusFlowOption { get; set; }
        public List<StatusFlowDTO>  StatusFlowDTO { get;set; }
        public List<PrivacyAccessDTO> PrivacyAccesses { get; set; }
        public List<ProjectCreatorDTO> ProjectCreators { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
