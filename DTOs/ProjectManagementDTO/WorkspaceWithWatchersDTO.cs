using System;
using HalobizMigrations.Models.ProjectManagement;

namespace HaloBiz.DTOs.ProjectManagementDTO
{
    public class WorkspaceWithWatchersDTO
    {
        public Workspace[] workspaces { get; set; }

        public long WatcherCount { get; set; }
    }
}
