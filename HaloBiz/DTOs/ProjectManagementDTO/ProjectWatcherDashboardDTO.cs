using System;
using HalobizMigrations.Models.ProjectManagement;

namespace HaloBiz.DTOs.ProjectManagementDTO
{
    public class ProjectWatcherDashboardDTO
    {
       public Workspace[] Workspace { get; set; }
       public int ProjectCount { get; set; }
    }
}
