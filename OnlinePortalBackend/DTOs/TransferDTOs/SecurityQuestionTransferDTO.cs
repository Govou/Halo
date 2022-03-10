using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePortalBackend.DTOs.TransferDTOs
{
    public class SecurityQuestionTransferDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public int HeadId { get; set; }
        public UserProfileTransferDTO Head { get; set; }
    }
}
