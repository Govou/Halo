using HaloBiz.Model.RoleManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.TransferDTOs.RoleManagement
{
    public class RoleClaimTransferDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long RoleId { get; set; }
    }
}
