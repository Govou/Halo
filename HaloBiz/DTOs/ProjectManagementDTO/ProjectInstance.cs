using System.Collections.Generic;
using HalobizMigrations.Models.ProjectManagement;

namespace HaloBiz.DTOs.ProjectManagementDTO
{
    public class ProjectInstance
    {
        public  List<Project> Projects { get; set; }
        public List<Task> Tasks { get; set; }
        
        public List<TaskAssignee> TaskAssignees { get; set; }
    }
}