using System;
using System.Collections.Generic;
using HalobizMigrations.Models.ProjectManagement;

namespace HaloBiz.DTOs.ProjectManagementDTO
{
    public class TaskRevampDTO
    {
        public long Id { get; set; }
        public long CreatedById { get; set; }
        public bool IsReassigned { get; set; }
        public bool IsAssigned { get; set; }
        public DateTime DueTime { get; set; }
        public int WorkingManHours { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsWorkbenched { get; set; }
        public string Caption { get; set; }
        public long? ProjectId { get; set; }
        public Project project { get; set; }
        public DateTime TaskEndDate { get; set; }
        public DateTime TaskStartDate { get; set; }
        public string Description { get; set; }
        public string Alias { get; set; }
        public bool IsPickedUp { get; set; }
        public long? TaskOwnershipId{get;set;}
        public List<TaskAssigneeDTO> TaskAssignees { get; set; }
        public List<Deliverable> Deliverables { get; set; }
        public bool IsMilestone { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
