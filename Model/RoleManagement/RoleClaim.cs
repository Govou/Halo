using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Model.RoleManagement
{
    public class RoleClaim
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public long RoleId { get; set; }
        public virtual Role Role { get; set; }

        public bool IsDeleted { get; set; }
    }
}
