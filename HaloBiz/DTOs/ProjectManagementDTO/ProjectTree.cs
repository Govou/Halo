using System;
using System.Collections.Generic;
using HalobizMigrations.Models.ProjectManagement;

namespace HaloBiz.DTOs.ProjectManagementDTO
{
    public class ProjectTree
    {
        public long Id { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }
        public string Alias { get; set; }
        public bool IsActive { get; set; }
        public string ProjectImage { get; set; }
        public long? WorkspaceId { get; set; }
        public List<TaskRevampDTO> Tasks { get; set; }
        public long CreatedById { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
