using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class SuspectReceivingDTO
    {
        public long GroupTypeId { get; set; }
        public string Tier { get; set; }
        public string RCNumber { get; set; }
        public string BusinessName { get; set; }
        public long? IndustryId { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string OtherName { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Street { get; set; }
        public long? LgaId { get; set; }
        public long? StateId { get; set; }
        public long? OfficeId { get; set; }
        public long? BranchId { get; set; }
        public long? LeadTypeId { get; set; }
        public long? LeadOriginId { get; set; }
        public string Address { get; set; }
        public string ImageUrl { get; set; }
        public bool isInternalClientType { get; set; } = false;
    }
}
