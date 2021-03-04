using System.Collections.Generic;
using HaloBiz.Model.LAMS;

namespace HaloBiz.DTOs.TransferDTOs.LAMS
{
    public class ClientBeneficiaryTransferDTO
    {
        public long Id { get; set; }
        public string BeneficiaryFamilyCode { get; set; }
        public string BeneficiaryCode { get; set; }
        public bool IsPrincipal { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public long MeansOfIdentifcationId { get; set; }
        public string IdentificationNumber { get; set; }
        public long? RelationshipId { get; set; }
        public long ClientId { get; set; }
        public CustomerDivision Client { get; set; }
        public string ImageUrl { get; set; }
        public long CreatedById { get; set; }
    }
}