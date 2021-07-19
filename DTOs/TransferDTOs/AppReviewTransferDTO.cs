using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class AppReviewTransferDTO
    {
        public long Id { get; set; }
        public string Module { get; set; }
        public long LookAndFeelRating { get; set; }
        public long FunctionalityRating { get; set; }
        public long CreatedById { get; set; }
    }
}
