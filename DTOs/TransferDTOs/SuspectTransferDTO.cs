using HalobizMigrations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class SuspectTransferDTO
    {
        public GroupType GroupType { get; set; }
        public long GroupTypeId { get; set; }
        public string Tier { get; set; }
        public string RCNumber { get; set; }
        public string BusinessName { get; set; }
        public Industry Industry { get; set; }
        public long? IndustryId { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string OtherName { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Street { get; set; }
        public Lga Lga { get; set; }
        public long? LgaId { get; set; }
        public State State { get; set; }
        public long? StateId { get; set; }
        public Office Office { get; set; }
        public long? OfficeId { get; set; }
        public Branch Branch { get; set; }
        public long? BranchId { get; set; }
        public LeadType LeadType { get; set; }
        public long? LeadTypeId { get; set; }
        public LeadOrigin LeadOrigin { get; set; }
        public long? LeadOriginId { get; set; }
        public long Id { get; set; }
        public string Address { get; set; }
    }
}
