using HalobizMigrations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class ProspectTransferDTO
    {
        public OfficeTransferDTO Office { get; set; }
        public BranchTransferDTO Branch { get; set; }
        public Lead Lead { get; set; }
        public UserProfileTransferDTO CreatedBy { get; set; }
        public long CreatedById { get; set; }
        public long? OfficeId { get; set; }
        public long? BranchId { get; set; }
        public Channel Channel { get; set; }
        public DateTime TentativeBusinessStartDate { get; set; }
        public string TentativeBugdet { get; set; }
        public string LogoUrl { get; set; }
        public string Address { get; set; }
        public string Street { get; set; }
        public long? StateId { get; set; }
        public long Id { get; set; }
        public long? LeadId { get; set; }
        public string Origin { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public StateTransferDTO State { get; set; }
        public string CompanyName { get; set; }
        public ProspectType ProspectType { get; set; }
        public string MeansOfIdentification { get; set; }
        public string IdentificationNumber { get; set; }
        public string Industry { get; set; }
        public string DivisionName { get; set; }
        public long? Lgaid { get; set; }
        public string Email { get; set; }
        public LGATransferDTO Lga { get; set; }
        public string IdentityUserId { get; set; }
        public string RCNumber { get; set; }
    }
}
