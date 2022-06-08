using System.Collections.Generic;
using HalobizMigrations.Models.ProjectManagement;

namespace HaloBiz.DTOs
{
    public class ProjectWatcherResponse
    {
        public List<Project> Project { get; set;}
        public List<Task> Task { get; set;}
    }
}