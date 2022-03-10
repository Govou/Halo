using HalobizMigrations.Models.OnlinePortal;
using OnlinePortalBackend.DTOs.AdapterDTOs;
using OnlinePortalBackend.DTOs.ReceivingDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Adapters
{
    public interface IPaymentAdapter
    {
        public Task<VerifyPaymentResponse> VerifyPaymentAsync(PaymentGateway paymentType, string referenceCode);
    }
}
