using HaloBiz.Model;
using HaloBiz.Model.LAMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.TransferDTOs.LAMS
{
    public class CustomerTransferDTO
    {
        public long Id { get; set; }
        public string GroupName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string RCNumber { get; set; }
        public long GroupTypeId { get; set; }
        public  GroupTypeTransferDTO GroupType { get; set; }
        public IEnumerable<CustomerDivisionTransferDTO> CustomerDivisions { get; set; }
    }
}
