using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.MailDTOs
{
    public class NewRoleAssignedDTO
    {
        public string EmailAddress { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public string[] RoleClaims { get; set; }
    }
}

