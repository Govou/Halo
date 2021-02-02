using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.TransferDTOs.RoleManagement
{
    public class RoleTransferDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<RoleClaimTransferDTO> RoleClaims { get; set; }
    }
}
