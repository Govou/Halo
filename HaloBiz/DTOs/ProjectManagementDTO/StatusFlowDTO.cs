using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.ProjectManagementDTO
{
    public class StatusFlowDTO
    {
        public long id { get; set; }
        public long LevelCount { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }
        public string Panthone { get; set; }
        public bool IsDeleted { get; set; }
        public long CreatedById { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
