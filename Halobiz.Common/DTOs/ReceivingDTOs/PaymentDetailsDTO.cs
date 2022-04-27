using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halobiz.Common.DTOs.ReceivingDTOs
{
    public class PaymentDetailsDTO
    {
        public int userId { get; set; }
        public string PaymentGateway { get; set; }
        public string PaymentReferenceInternal { get; set; }
        public string PaymentReferenceGateway { get; set; }
        public string SessionId { get; set; }
        public string PaymentGatewayResponseCode { get; set; }
        public string PaymentGatewayResponseDescription { get; set; }
        public decimal Value { get; set; }
        public decimal VAT { get; set; }
        public decimal ConvenienceFee { get; set; }
        public int TotalValue { get; set; }
        public string TransactionSource { get; set; }
        public string TransactionType { get; set; }
        public bool PaymentConfirmation { get; set; }
        public bool PaymentFulfilment { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int CreatedById { get; set; }
    }
}

