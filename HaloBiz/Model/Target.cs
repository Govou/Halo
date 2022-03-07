using System.Collections.Generic;

namespace HaloBiz.Model
{
    public class Target : SetupBaseModel
    {
        public IEnumerable<Services> Services { get; set; }
    }
}