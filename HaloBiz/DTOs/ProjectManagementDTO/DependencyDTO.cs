using HalobizMigrations.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.ProjectManagementDTO
{
    public class DependencyDTO
    {

        public long? DependencyDeliverableId { get; set; }
        
        public long? DependencyProfileId { get; set; }
        
        public long DeliverableDependentOnId { get; set; }
        public long CreatedById { get; set; }
  
    }
}
