using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Model
{
    public class AuthorizationExemption
    {
        public string Controller { get; set; }
        public List<string> Endpoints { get; set; } = new List<string>();
        public List<string> ActionVerbs { get; set; } = new List<string>();
    }

}
