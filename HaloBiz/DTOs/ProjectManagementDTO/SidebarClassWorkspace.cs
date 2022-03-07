using System;
using System.Collections.Generic;
using HalobizMigrations.Models.ProjectManagement;

namespace HaloBiz.DTOs.ProjectManagementDTO
{
    public class SidebarClassWorkspace
    {
        public List<Workspace> workspace { get; set; }
        public List<Project> project { get; set; }
        public List<Task> task { get; set; }
        public List<Deliverable> deliverables { get; set; }
    }
}
