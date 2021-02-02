using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Model.RoleManagement
{
    public class Role
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Role name should only contain characters with no white space"), MaxLength(40)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
        public virtual ICollection<RoleClaim> RoleClaims { get; set; }
    }
}
