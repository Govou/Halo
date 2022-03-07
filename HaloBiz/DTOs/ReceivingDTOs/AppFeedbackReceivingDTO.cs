using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class AppFeedbackReceivingDTO
    {
        [Required]
        public string FeedbackType { get; set; }
        public string Description { get; set; }
        public string DocumentsUrl { get; set; }
    }
}
