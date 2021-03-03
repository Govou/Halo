using System.ComponentModel.DataAnnotations;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class ClientBeneficiaryReceivingDTO
    {
        [Required]
        public string BeneficiaryFamilyCode { get; set; }
        [Required]
        public string BeneficiaryCode { get; set; }
        public bool IsPrincipal { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string MiddleName { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public long MeansOfIdentifcationId { get; set; }
        public string IdentificationNumber { get; set; }
        public long RelationshipId { get; set; }
        [Required]
        public long ClientId { get; set; }
        public string ImageUrl { get; set; }
    }
}