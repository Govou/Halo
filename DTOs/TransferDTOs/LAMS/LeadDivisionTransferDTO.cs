using System.Collections.Generic;
using HalobizMigrations.Models;


namespace HaloBiz.DTOs.TransferDTOs.LAMS
{
    public class LeadDivisionTransferDTO
    {
        public long Id { get; set; }
        public long LeadTypeId { get; set; }
        public LeadTypeWithoutOriginDTO LeadType { get; set; }
        public long LeadOriginId { get; set; }
        public LeadOriginWithoutTypeTransferDTO LeadOrigin { get; set; }
        public string Industry { get; set; }
        public string RCNumber { get; set; }
        public string DivisionName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        #region Address is a concatenation of State, LGA and Street for each lead Division. Address will be auto populated
        public long? StateId { get; set; }
        public long? LGAId { get; set; }
        public string Street { get; set; }
        #endregion
        public string LogoUrl { get; set; }
        public string Address { get; set; }
        public long PrimaryContactId { get; set; }
        public LeadDivisionContactTransferDTO PrimaryContact { get; set; }
        public long? SecondaryContactId { get; set; }
        public LeadDivisionContactTransferDTO SecondaryContact { get; set; }
        public long BranchId { get; set; }
        public BranchTransferDTO Branch { get; set; }
        public long OfficeId { get; set; }
        public virtual OfficeTransferDTO Office { get; set; }
        public long LeadId { get; set; }
        public LeadTransferDTO Lead { get; set; }
        public QuoteWithoutLeadDivisionTransferDTO Quote { get; set; }
        public IEnumerable<LeadDivisionKeyPersonTransferDTO> LeadDivisionKeyPersons { get; set; }
        public long CreatedById { get; set; }
        public virtual UserProfile CreatedBy { get; set; }
    }
}