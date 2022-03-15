using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Model
{
    public class LoginFailureTracker
    {
        public string Email { get; set; }
        public int Count { get; set; }
        public DateTime LockedTime { get; set; }
    }
}
