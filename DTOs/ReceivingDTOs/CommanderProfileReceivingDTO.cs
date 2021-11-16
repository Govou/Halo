using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class CommanderProfileReceivingDTO
    {
        [Required]
        public long? CommanderTypeId { get; set; }
        [Required]
        public long? AttachedOfficeId { get; set; }
        [Required]
        public long? AttachedBranchId { get; set; }
        [Required]
        public long ProfileId { get; set; }
        [Required]
        public long? RankId { get; set; }
    }
}
