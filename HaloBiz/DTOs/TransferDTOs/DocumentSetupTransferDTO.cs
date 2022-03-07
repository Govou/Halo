using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class DocumentSetupTransferDTO
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public string Caption { get; set; }
        public string DocumentUrl { get; set; }
    }
}
