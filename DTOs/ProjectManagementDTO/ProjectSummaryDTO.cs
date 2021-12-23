using HalobizMigrations.Models.ProjectManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task = HalobizMigrations.Models.ProjectManagement.Task;

namespace HaloBiz.DTOs.ProjectManagementDTO
{
    public class ProjectSummaryDTO
    {
        public long Id { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }
        public string Alias { get; set; }
        public bool IsActive { get; set; }
        public string ProjectImage { get; set; }
        public long? WorkspaceId { get; set; }
        public Workspace Workspace { get; set; }
        public ICollection<Watcher> Watchers { get; set; }

        public ICollection<Task> Tasks { get; set; }
        public long CreatedById { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
