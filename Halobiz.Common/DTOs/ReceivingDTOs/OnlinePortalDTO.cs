using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halobiz.Common.DTOs.ReceivingDTOs
{
    public class OnlinePortalDTO
    {
        public string Salutation { get; set; }
        public string Name { get; set; }
        public List<string> DetailsInPara { get; set; }
        public string[] Recepients { get; set; }
        public string Subject { get; set; }
    }
}
