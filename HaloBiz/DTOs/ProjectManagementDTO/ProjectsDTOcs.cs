using HalobizMigrations.Models.ProjectManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.ProjectManagementDTO
{
    public class ProjectsDTOcs
    {
        public string Caption { get; set; }
        public string Description { get; set; }
        public string Alias { get; set; }
        public string ProjectImage { get; set; }
        public bool IsActive { get; set; }
        public long? WorkspaceId { get; set; }
        
        public long? ProjectCreatorId { get; set; }
       
        //public ICollection<Watcher> Watchers { get; set; }
        public long CreatedById { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
