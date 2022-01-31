using HalobizMigrations.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.TransferDTOs
{   
    public class ServiceRelationshipDTO
    {
        [Key]
        public long Id { get; set; }
        public long ServiceAdminId { get; set; }
        public long ServiceDirectId { get; set; }
        public Service ServiceAdmin { get; set; }
        public Service ServiceDirect { get; set; }

        public Service AdminService { get; set; }
        public Service DirectService { get; set; }
    }
}
