using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.MailDTOs
{
    public class NewDeliverableAssignedDTO
    {
        public string EmailAddress { get; set; }
        public string UserName { get; set; }
        public string Customer { get; set; }
        public string TaskOwner { get; set; }
        public string TaskName { get; set; }
        public string ServiceName { get; set; }
        public string Quantity { get; set; }
        public string Deliverable { get; set; }
    }
}

