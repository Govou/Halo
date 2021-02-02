using System.Collections.Generic;
using HaloBiz.Model;
using HaloBiz.Model.LAMS;

namespace HaloBiz.DTOs.TransferDTOs.LAMS
{
    public class LeadDivisionTransferDTO
    {
        public long Id { get; set; }
        public long LeadOriginId { get; set; }
        public virtual LeadOrigin LeadOrigin { get; set; }
        public string Industry { get; set; }
        public string RCNumber { get; set; }
        public string DivisionName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string LogoUrl { get; set; }
        public string Address { get; set; }
        public long PrimaryContactId { get; set; }
        public virtual LeadDivisionContact PrimaryContact { get; set; }
        public long? SecondaryContactId { get; set; }
        public LeadDivisionContact SecondaryContact { get; set; }
        public long BranchId { get; set; }
        public virtual Branch Branch { get; set; }
        public long OfficeId { get; set; }
        public virtual Office Office { get; set; }
        public long LeadId { get; set; }
        public virtual Lead Lead { get; set; }
        public IEnumerable<Quote> Quotes { get; set; }
        public IEnumerable<LeadDivisionKeyPerson> LeadDivisionKeyPersons { get; set; }
        public long CreatedById { get; set; }
        public virtual UserProfile CreatedBy { get; set; }
    }
}