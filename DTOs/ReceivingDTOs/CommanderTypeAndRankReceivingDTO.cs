using HalobizMigrations.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class CommanderTypeAndRankReceivingDTO
    {
        [Required]
        public string TypeName { get; set; }
        [Required]
        public string TypeDesc { get; set; }
        public long ServiceRegistrationId { get; set; }
        //public long CreatedById { get; set; }

    }

    public class CommanderRankReceivingDTO
    {
     
     
     
        [Required]
        //public long Sequence { get; set; }
      
        public string Description { get; set; }
        [Required]
        public string Alias { get; set; }
        [Required]
        public string RankName { get; set; }
       [Required]
        public long? CommanderTypeId { get; set; }
    }
}
