using HalobizMigrations.Models.ProjectManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.ProjectManagementDTO
{
    public class RevampedWorkspaceDTO
    {
        public long Id { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }
        public string IsPublic { get; set; }
        public string Alias { get; set; }
        public bool IsActive { get; set; }
        public string StatusFlowOption { get; set; }
        public ICollection<StatusFlow> StatusFlowDTO { get; set; }
        public ICollection<PrivacyAccess> PrivacyAccesses { get; set; }
        public ICollection<ProjectCreator> ProjectCreators { get; set; }
        public long ProjectCreatorsLength { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
