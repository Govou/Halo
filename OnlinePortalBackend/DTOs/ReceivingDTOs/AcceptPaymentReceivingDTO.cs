using HalobizMigrations.Models.OnlinePortal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePortalBackend.DTOs.ReceivingDTOs
{
    public class AcceptPaymentReceivingDTO
    {
        public long ContractServiceId { get; set; }
        [Required]
        public string ReferenceCode { get; set; }
        public PaymentGateway PaymentType { get; set; }
    }
}
