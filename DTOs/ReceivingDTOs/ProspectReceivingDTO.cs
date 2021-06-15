using HalobizMigrations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class ProspectReceivingDTO
    {
        public long? OfficeId { get; set; }
        public long? BranchId { get; set; }
        public Channel Channel { get; set; }
        public DateTime TentativeBusinessStartDate { get; set; }
        public string TentativeBugdet { get; set; }
        public string LogoUrl { get; set; }
        public string Address { get; set; }
        public string Street { get; set; }
        public long? StateId { get; set; }
        public long? LeadId { get; set; }
        public string Origin { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public string CompanyName { get; set; }
        public ProspectType ProspectType { get; set; }
        public string MeansOfIdentification { get; set; }
        public string IdentificationNumber { get; set; }
        public string Industry { get; set; }
        public string DivisionName { get; set; }
        public long? Lgaid { get; set; }
        public string Email { get; set; }
        public string IdentityUserId { get; set; }
    }
}
