using HaloBiz.Model.RoleManagement;
using HalobizMigrations.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.TransferDTOs.RoleManagement
{
    public class ClaimTransferDTO
    {
        public string Description { get; set; }
        public string Name { get; set; }
        public ClaimEnum ClaimEnum { get; set; }
    }
}
