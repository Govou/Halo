using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePortalBackend.DTOs.AdapterDTOs
{
    public class VerifyPaymentResponse
    {
        public bool PaymentSuccessful { get; set; }
        public decimal PaymentAmount { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}