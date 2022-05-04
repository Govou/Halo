using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Model
{
    public class AdminRole
    {         
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
        public List<string> RoleAssignees { get; set; } = new List<string>();
    }
}
