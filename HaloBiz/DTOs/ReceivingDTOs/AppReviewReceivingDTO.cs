using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class AppReviewReceivingDTO
    {
        [Required]
        public string Module { get; set; }
        public long LookAndFeelRating { get; set; }
        public long FunctionalityRating { get; set; }
    }
}
