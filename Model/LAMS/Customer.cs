using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Model.LAMS
{
    public class Customer
    {
        [Key]
        public long Id { get; set; }
        [Required, MinLength(3), MaxLength(100)]
        public string GroupName { get; set; }
        [Required, RegularExpression("\\d{10,15}")]
        public string PhoneNumber { get; set; }
        [Required, RegularExpression("^[_A-Za-z0-9-\\+]+(\\.[_A-Za-z0-9-]+)*@[A-Za-z0-9-]+(\\.[A-Za-z0-9]+)*(\\.[A-Za-z]{2,})$", ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [StringLength(100)]
        public string Industry { get; set; }
        [Required]
        public string RCNumber { get; set; }
        [Required]
        public string LogoUrl { get; set; }
        public long GroupTypeId { get; set; }
        public virtual GroupType GroupType { get; set; }
        public IEnumerable<CustomerDivision> CustomerDivisions { get; set; }
        [Required]
        public long CreatedById { get; set; }
        public virtual UserProfile CreatedBy { get; set; }
        public bool IsDeleted { get; set; } = false;
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }

        
    }
}
