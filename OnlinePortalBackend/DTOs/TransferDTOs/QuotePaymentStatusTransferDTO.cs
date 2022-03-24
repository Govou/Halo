using HalobizMigrations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePortalBackend.DTOs.TransferDTOs
{
    public class QuotePaymentStatusTransferDTO
    {
        public Quote Quote { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
    }

    public enum PaymentStatus
    {
        Pending = 1,
        Success,
        Failed
    }
}
