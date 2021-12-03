using HalobizMigrations.Models.ProjectManagement;
using HalobizMigrations.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task = HalobizMigrations.Models.ProjectManagement.Task;

namespace HaloBiz.DTOs.ProjectManagementDTO
{
    public class DeliverableDTO
    {

        public long CreatedById { get; set; }
        public DependentType DependentType { get; set; }
        public ICollection<DependencyDTO> Dependencies { get; set; }

        public ICollection<RequirementsDTO> Requirements { get; set; }
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
        public DateTime UpdatedAt { get; set; }

    }
}
