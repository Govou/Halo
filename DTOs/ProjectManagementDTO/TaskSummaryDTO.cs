using HalobizMigrations.Models.ProjectManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.ProjectManagementDTO
{
    public class TaskSummaryDTO
    {
        public long CreatedById { get; set; }
        public ICollection<TaskAssignee> TaskAssignees { get; set; }
        public List<Deliverable> Deliverables { get; set; }
        public bool IsReassigned { get; set; }
        public bool IsAssigned { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsWorkbenched { get; set; }
        public string Caption { get; set; }
        public long? ProjectId { get; set; }
        public DateTime TaskEndDate { get; set; }
        public DateTime TaskStartDate { get; set; }
        public string Description { get; set; }
        public string Alias { get; set; }
        public bool IsMilestone { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Workspace workspace { get; set; }

        public Project Project { get; set; }
    }
}
