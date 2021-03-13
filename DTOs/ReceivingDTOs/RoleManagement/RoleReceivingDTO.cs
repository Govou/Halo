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
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public List<RoleClaimReceivingDTO> RoleClaims { get; set; }
    }
}
