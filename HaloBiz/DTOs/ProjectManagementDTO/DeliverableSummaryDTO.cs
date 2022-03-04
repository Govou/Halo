using HalobizMigrations.Models.ProjectManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task = HalobizMigrations.Models.ProjectManagement.Task;

namespace HaloBiz.DTOs.ProjectManagementDTO
{
    public class DeliverableSummaryDTO
    {

        
        public long CreatedById { get; set; }

        public ICollection<Dependency> Dependencies { get; set; }
        public DateTime CreatedAt { get; set; }
        public long TimeEstimate { get; set; }
        public long? TaskId { get; set; }
        public DateTime DatePicked { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime StartDate { get; set; }
        public decimal Budget { get; set; }
        public string Description { get; set; }
        public string Caption { get; set; }
        public string Alias { get; set; }
        public Task Task { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
