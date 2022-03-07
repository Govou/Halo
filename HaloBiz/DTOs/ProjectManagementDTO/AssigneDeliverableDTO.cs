using System;
using System.Collections.Generic;
using HalobizMigrations.Models.ProjectManagement;

namespace HaloBiz.DTOs.ProjectManagementDTO
{
    public class AssigneDeliverableDTO
    {
        public Project[] Project { get; set; }
        public int DeliverableCount { get; set; }
    }
}
