using System.Collections.Generic;
using HalobizMigrations.Models;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class StateTransferDTO
    {
        public long Id { get; set; }
        public int StateCode { get; set; }
        public string Name { get; set; }
        public string Capital { get; set; }
        public string  Slogan { get; set; }
        public IEnumerable<LGATransferDTO> LGAs { get; set; }
    }
}