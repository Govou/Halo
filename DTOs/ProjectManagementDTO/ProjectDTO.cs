using HalobizMigrations.Models.ProjectManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.ProjectManagementDTO
{
    public class ProjectDTO
    {
        public string Caption { get; set; }
        public string Description { get; set; }
        public string Alias { get; set; }
        public string ProjectImage { get; set; }
        public long? WorkspaceId { get; set; }
        public ICollection<WatchersDTO> Watchers { get; set; }
        public long CreatedById { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
