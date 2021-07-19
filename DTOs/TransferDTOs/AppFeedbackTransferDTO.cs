using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class AppFeedbackTransferDTO
    {
        public long Id { get; set; }
        public string FeedbackType { get; set; }
        public string Description { get; set; }
        public string DocumentsUrl { get; set; }
        public long CreatedById { get; set; }
    }
}
