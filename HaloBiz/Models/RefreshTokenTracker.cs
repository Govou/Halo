using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Models
{
    public class RefreshTokenTracker
    {
        public string Id { get; set; }
        public string Token { get; set; }
        public DateTime GracePeriod { get; set; }
    }
}
