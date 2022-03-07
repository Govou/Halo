using System.Collections.Generic;

namespace HaloBiz.Model
{
    public class ServiceType : SetupBaseModel
    {
        public IEnumerable<Services> Services { get; set; }
    }
}