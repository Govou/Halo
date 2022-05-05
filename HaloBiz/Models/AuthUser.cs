using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Models
{
    public class AuthUser
    {
        public string Email { get; set; }
        public string Id { get; set; }
        public string permissionString { get; set; }
        public bool hasAdminRole { get; set; }
    }
}
