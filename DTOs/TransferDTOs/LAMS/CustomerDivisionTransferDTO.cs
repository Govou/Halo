using HaloBiz.DTOs.TransferDTOs.LAMS;
using HaloBiz.Model.LAMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class CustomerDivisionTransferDTO
    {
        public long Id { get; set; }
        public string Industry { get; set; }
        public string RCNumber { get; set; }
        public string DivisionName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string LogoUrl { get; set; }
        public string Address { get; set; }
        public long CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public IEnumerable<AccountMasterTransferDTO> AccountMaster { get; set; }
        public List<LeadKeyPerson> LeadKeyPeople { get; set; } 
        public LeadDivisionContact PrimaryContact { get; set; }
        public IEnumerable<ContractForCustomerDivisionTransferDTO> Contracts { get; set; }
        public long? PrimaryContactId { get; set; }   
        public LeadDivisionContact SecondaryContact { get; set; }
        public long? SecondaryContactId { get; set; } 

    }
    public class CustomerDivisionWithoutObjectsTransferDTO
    {
        public long Id { get; set; }
        public string Industry { get; set; }
        public string RCNumber { get; set; }
        public string DivisionName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string LogoUrl { get; set; }
        public string Address { get; set; }
        public long CustomerId { get; set; }

    }
}
