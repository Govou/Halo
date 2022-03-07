using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class DocumentReceivingDTO
    {
        [StringLength(1000)]
        public string Description { get; set; }
        [StringLength(100)]
        public string Caption { get; set; }
        [StringLength(500)]
        public string DocumentUrl { get; set; }
    }
}
