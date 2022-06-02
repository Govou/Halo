using System.Collections.Generic;
using HalobizMigrations.Models.ProjectManagement;

namespace HaloBiz.DTOs
{
    public class ProjectExtract
    {
        public List<Project> Projects { get; set; }
        public List<Deliverable> Deliverables { get; set; }
        public List<Task> Tasks { get; set;}
        
        public List<StatusFlow> StatusFlows { get; set; }
    }
}