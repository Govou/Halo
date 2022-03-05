using System;
using System.Collections.Generic;
using HalobizMigrations.Models.ProjectManagement;

namespace HaloBiz.DTOs.ProjectManagementDTO
{
    public class workspaceWithStatusDTO
    {
        public long Id { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }
        public string Alias { get; set; }
        public bool IsActive { get; set; }
        public string StatusFlowOption { get; set; }
        public ICollection<statusWithDeliverable> statusFlows { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
