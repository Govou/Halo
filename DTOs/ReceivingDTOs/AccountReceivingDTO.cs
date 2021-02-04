using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class AccountReceivingDTO
    {
        [Required]
        public string Name { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        [Required]
        public string Alias { get; set; }
        [Required]
        public bool IsDebitBalance { get; set; }
        [Required]
        public long ControlAccountId { get; set; }
    }
}
