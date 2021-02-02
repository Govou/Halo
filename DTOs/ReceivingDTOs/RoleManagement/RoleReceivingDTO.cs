using HaloBiz.Model.RoleManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.ReceivingDTOs.RoleManagement
{
    public class RoleReceivingDTO
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Role name should only contain characters with no white space"), MaxLength(40)]
        public string Name { get; set; }

        public virtual ICollection<RoleClaimReceivingDTO> RoleClaims { get; set; }
    }
}
