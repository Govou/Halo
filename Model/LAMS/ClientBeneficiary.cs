using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Model.LAMS
{
    public class ClientBeneficiary
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public string BeneficiaryFamilyCode { get; set; }
        [Required]
        public string BeneficiaryCode { get; set; }
        [Required]
        public bool IsPrincipal { get; set; }
        [Required, MinLength(1), MaxLength(255)]
        public string Title { get; set; }
        [Required, MinLength(1), MaxLength(255)]
        public string FirstName { get; set; }
        [Required, MinLength(1), MaxLength(255)]
        public string LastName { get; set; }
        [Required, MinLength(1), MaxLength(255)]
        public string MiddleName { get; set; }
        public string Mobile { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Address { get; set; }
        public long MeansOfIdentifcationId { get; set; }
        public string IdentificationNumber { get; set; }
        public long RelationshipId { get; set; }
        [Required]
        public long ClientId { get; set; }
        public virtual CustomerDivision Client { get; set; }
        public string ImageUrl { get; set; }
        public bool IsDeleted { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }
    }
}
