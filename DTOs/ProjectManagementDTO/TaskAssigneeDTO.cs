using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.ProjectManagementDTO
{
    public class TaskAssigneeDTO
    {
        public string Name { get; set; }
        public long? TaskId { get; set; }
       
        public bool IsActive { get; set; }
        public long TaskAssigneeId { get; set; }
        public long CreatedById { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
