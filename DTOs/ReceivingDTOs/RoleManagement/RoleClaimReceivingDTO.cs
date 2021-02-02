using HaloBiz.Model.RoleManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.ReceivingDTOs.RoleManagement
{
    public class RoleClaimReceivingDTO
    {
        [Required]
        public string Name { get; set; }
    }
}
