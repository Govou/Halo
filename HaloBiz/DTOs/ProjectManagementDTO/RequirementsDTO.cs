using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.ProjectManagementDTO
{
    public class RequirementsDTO
    {

        public string Caption { get; set; }
        public string Alias { get; set; }
        public string Descrption { get; set; }
        public string FileExtention { get; set; }
        public long? DeliverableId { get; set; }
        public long CreatedById { get; set; }

    }
}
