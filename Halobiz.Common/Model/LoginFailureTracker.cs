using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Halobiz.Common.Model
{
    public class LoginFailureTracker
    {
        public string Email { get; set; }
        public DateTime LockedExpiration { get; set; }
    }
}
