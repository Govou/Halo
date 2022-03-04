using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class NoteTransferDTO
    {
        public long Id { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }
        public string Subject { get; set; }
        public long CreatedById { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
