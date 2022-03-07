using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class LoginTransferDTO
    {
        public string Token { get; set; }
        public DateTime ExpiryDate { get; set; }

    }
}
