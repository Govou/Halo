using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class DTrackCustomerCreate
    {
        public string CustomerNumber { get; set; }
        public string Name { get; set; }
        public string GLAccount { get; set; }
        public string ShortName { get; set; }
        public string AddressLine1 { get; set; }
        public string EmailAddress { get; set; }
        public string TelephoneNumber { get; set; }
        public string Location { get; set; }
        public string BusinessSector { get; set; }
        public string OtherInfo { get; set; }
        public string Contact { get; set; }
    }
}
